# Desafio do PicPay Simplificado

[![Tests](https://github.com/ZaqueuCavalcante/picpay/actions/workflows/tests.yml/badge.svg)](https://github.com/ZaqueuCavalcante/picpay/actions/workflows/tests.yml)

Essa é minha resolução do [desafio backend proposto pelo PicPay](https://github.com/PicPay/picpay-desafio-backend):

- O PicPay Simplificado é uma plataforma de pagamentos apenas com funcionalidades básicas.
- Nela é possível depositar e realizar transferências de dinheiro entre usuários.
- Temos 2 tipos de usuários, Clientes e Lojistas, ambos têm carteira com dinheiro e podem realizar transferências.
- A realização de transferências depende de um serviço autorizador externo, que determina se a operação pode acontecer ou não.
- Em caso de sucesso da transferência, o recebedor deve ser notificado da transação, através de um serviço externo de notificação.
- Ambos os serviços externos podem estar estar indisponíveis no momento que são chamados (precisamos tratar esses casos no código).

Temos 3 pontos principais neste projeto:

- A consistência dos dados é fundamental (o dinheiro não pode sumir nem surgir do nada)
- A segurança dos dados também é fundamental (apenas você pode transferir/consultar seu dinheiro)
- O envio de notificações deve ser feito de maneira assíncrona, tornando o sistema resiliente a falhas

Resumo do que você vai encontrar aqui:

- API C#/.Net + documentação com Scalar
- Mais de 100 testes automatizados
- CI/CD com o GitHub Actions
- Deploy no Railway (Api + Postgres + Worker)
- Processamento assíncrono de eventos e tarefas em background
- Gestão de erros e reprocessamento de eventos/tarefas
- Concorrência e paralelismo + consistência financeira

## 0️⃣ Sumário

- 1️⃣ Regras de Negócio
- 2️⃣ Arquitetura
- Casos de Uso
- Testes
- Referências

## 1️⃣ Regras de Negócio

- Existem dois tipos de usuários no sistema: Clientes e Lojistas

- Usuários possuem Nome, Documento, E-mail e Senha:
    - Documentos e e-mails devem ser únicos no sistema
    - O sistema deve permitir apenas um cadastro com o mesmo Documento ou E-mail

- Clientes podem realizar transferências para Lojistas ou outros Clientes

- Lojistas só recebem transferências, não enviam

- Antes de finalizar a transferência, deve-se consultar um serviço autorizador externo (https://util.devi.tools/api/v2/authorize)

- Após a transferência:
    - O cliente ou lojista precisa receber uma notificação (email, sms) enviada por um serviço externo (https://util.devi.tools/api/v1/notify)
    - Eventualmente este serviço pode estar indisponível/instável

> 🆙 As regras a seguir foram adicionadas por mim, para deixar o projeto mais desafiador

- Existe um terceiro tipo de usuário no sistema, o Adm:
    - Ele será responsável pela gestão do sistema como um todo, tendo acesso aos dados de clientes e lojistas

- A API terá autenticação e autorização:
    - Endpoins para cadastro de usuários e para realização de login

- A notificação será enviada de maneira assíncrona:
    - Teremos um evento de Transferência Realizada, que vai disparar uma tarefa em background para enviar a notificação
    - Vamos usar uma política de retentativa simples, caso o serviço externo retorne algum erro

- Bônus de Boas-Vindas:
    - Todo Cliente que se cadastrar no PicPay, receberá um bônus no valor de R$ 10,00

- Cliente deve poder acessar:
    - Seu saldo atual
    - Extrato com todas as suas transações
    - Listagem com todas as suas notificações

- Lojista deve poder acessar:
    - Relatórios de transações, com filtros de datas, valores e clientes
    - Relatório de notificações, com pendentes, sucessos e erros

- Adm deve poder acessar:
    - Dados de consistência financeira
    - Informações sobre Clientes e Lojistas

## 2️⃣ Arquitetura

- Api
- Worker
- Vendors
- Postgres

* Eventos + Tasks
* Diagrama do banco de dados


## Casos de Uso

Casos de uso mapeados, facilitando a implementação e os testes.

### Cadastro de Clientes ou Lojistas
- Ver se o documento é válido
- Ver se o email é válido
- Ver se a senha informada é forte
- Ver se o documento ou email já está sendo usado por outro usuário
- Dois requests com os mesmos dados feitos no mesmo instante devem cadastrar apenas um usuário

### Login
- Não deve logar quando o usuário não existir
- Não deve logar quando a senha estiver incorreta

### Bônus de Boas-Vindas
- Clientes novos devem receber um bônus de R$ 10,00
- A criação de clientes em paralelo deve manter os saldos consistentes
- Uma notificação deve ser enviada com a menssagem: "Bônus de Boas-Vindas no valor de R$ 10,00"
- Lojistas não devem receber esse bônus

### Transferência de dinheiro
- Chamada não autenticada deve receber 403

- Apenas Clientes podem transferir
    - Adm deve receber 401
    - Lojista deve receber 401

- Não pode transferir valor <= 0

- Nâo pode transferir pra uma Carteira inexistente

- Não pode transferir para si próprio

- Não pode transferir sem saldo suficiente

- Não pode transferir caso seja não autorizado

- Não pode transferir caso o autorizador esteja fora do ar

- Não deve transferir em paralelo, gastando mais que o saldo
    - Tenho R$ 10,00 de saldo, não posso realizar duas transferências de R$ 8,00 ao mesmo tempo
    - Apenas uma delas deve ser feita, a outra deve retornar erro
    - Se forem duas de R$ 4,00 ambas devem ser realizadas com sucesso

- Quem recebe duas transferências ao mesmo tempo deve acabar com o saldo correto
    - Um lojista recebendo vários pagamentos de maneira simultânea deve se manter com saldo consistente
    - O lojista possui saldo de R$ 5,00
    - Dois clientes transferem R$ 8,00 cada pro lojista ao mesmo tempo
    - O saldo final do lojista deve ser de R$ 21,00

- Quem enviar pra X e receber de Y ao mesmo tempo deve acabar com o saldo correto
    - Maria possui R$ 5,00 de saldo, Zezinho possui R$ 8,00 e o lojista Gilbirdelson possui R$ 10,00
    - Maria envia R$ 2,00 pra Gilbirdelson e, no exato mesmo instante, Zezinho envia R$ 6,00 pra Maria
    - Maria deve terminar com saldo de R$ 9,00
    - Gilbirdelson com R$ 12,00
    - Zezinho com R$ 2,00

- Quem enviar pra X e receber de X ao mesmo tempo deve acabar com o saldo correto
    - Maria possui R$ 10,00 de saldo e Zezinho possui R$ 10,00
    - Maria envia R$ 6,00 pra Zezinho e, no exato mesmo instante, Zezinho envia R$ 2,00 pra Maria
    - Maria deve terminar com saldo de R$ 6,00 e Zezinho com R$ 14,00
    - Se não tratado, isso pode gerar um deadlock no banco de dados

- Cliente pode transferir para um lojista

- Cliente pode transferir para outro cliente

- Ao final, o recebedor (cliente ou lojista) deve ser notificado da transação

### Notificações do Usuário
- Listar todas as notificações do usuário, ordenadas pela mais recente

### Extrato de Transações
- Listar todas as transações do usuário, ordenadas pela mais recente

### Auditoria
- Quem, fez o quê, onde e quando?
- Tudo deve ser salvo para fins de histórico e auditoria





## Testes

- Adicionar description nos testes, mostrar no log de execução
- Testes com as APIs externas fora do ar (timeout baixo)
- Testes de carga com o K6 (quantas transações podem ser efetuadas por s/min/h/dia)

- Uso do pattern Facade nos testes, abstraindo as chamadas pra API
- Para cada caso de uso, colocar um print ou code sniped do teste

- Auth e Notify devem se comportar dinâmicamente
    - Passo algum parâmetro na query string que altera o valor do retorno no endpoint






## Referências

- Repositório original do desafio
    - https://github.com/PicPay/picpay-desafio-backend

- Descomplicando Clean Architecture, DDD e TDD com Desafio PicPay
    - O Nonato faz uma análise excelente dos requisitos e monta casos de uso/teste detalhados
    - https://youtu.be/u-jkRk-kKUM

- Picpay simplificado com Java e Spring Boot!
    - A Giuliana Bezerra desenvolve uma arquitetura robusta, com entrega assícrona das notificações
    - https://youtu.be/YcuscoiIN14

## Final

- Comente se vc conhece mais desafios de empresas
- O que faltou no projeto?
- Faca um fork do projeto e teste na sua maquina!
- Contribua com o projeto, abra um PR la!
