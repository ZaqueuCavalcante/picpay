# PicPay

[![Tests](https://github.com/ZaqueuCavalcante/picpay/actions/workflows/tests.yml/badge.svg)](https://github.com/ZaqueuCavalcante/picpay/actions/workflows/tests.yml)

Desafio do PicPay simplificado, feito em C# | DotNet

## Sumário

- Contexto
- Regras de Negócio
- Casos de Uso
- Testes
- Arquitetura
- Referências

## 0 - Contexto

Essa é minha resolução do desafio proposto pelo PicPay, que pode ser acessado [aqui](https://github.com/PicPay/picpay-desafio-backend).

O PicPay Simplificado é uma plataforma de pagamentos simplificada.
Nela é possível depositar e realizar transferências de dinheiro entre usuários.
Temos 2 tipos de usuários, os comuns e lojistas, ambos têm carteira com dinheiro e realizam transferências entre eles.

## Regras de Negócio

- Existem dois tipos de usuários no sistema
    - Clientes
    - Lojistas

- Usuários possuem Nome, Documento, E-mail e Senha
    - CPF/CNPJ e e-mails devem ser únicos no sistema
    - O sistema deve permitir apenas um cadastro com o mesmo Documento ou E-mail

- Clientes podem enviar dinheiro (efetuar transferência)
    - Para lojistas
    - Para outros clientes

- Lojistas só recebem transferências, não enviam dinheiro para ninguém

- Validar se o cliente tem saldo antes da transferência

- Antes de finalizar a transferência
    - Deve-se consultar um serviço autorizador externo
    - Use este mock https://util.devi.tools/api/v2/authorize para simular o serviço utilizando o verbo GET

- A operação de transferência deve ser uma transação
    - Ou seja, deve ser revertida em qualquer caso de inconsistência

- No recebimento de pagamento
    - O cliente ou lojista precisa receber notificação (envio de email, sms) enviada por um serviço de terceiro
    - Eventualmente este serviço pode estar indisponível/instável
    - Use este mock https://util.devi.tools/api/v1/notify para simular o envio da notificação utilizando o verbo POST

> As seguintes regras eu que adicionei para deixar o projeto mais desafiador

- Existe um terceiro tipo de usuário no sistema, o Adm
    - Ele será responsável pela gestão do sistema como um todo, tendo acesso aos dados de clientes e lojistas

- A API terá autenticação e autorização
    - Endpoins para cadastro de clientes e lojistas
    - Endpoint para login
    - Garantir via testes de segurança que nenhum dado alheio possa ser acessado

- Feature de depósito
    - O Adm pode realizar um depósito para uma carteira qualquer

- A notificação será feita de maneira assíncrona
    - Evento de pagamento efetuado
    - Task para enviar email, sms, notification
    - Retry automático de reenvio com delay
    - Garantir que apenas uma transação seja enviada

- Deve existir um front-end
    - Clientes, Lojistas e Adm vão acessar
    - Deve respeitar todas as políticas de acesso da API
    - Fazer na mesma paleta de cores do PicPay

- Cliente deve poder acessar
    - Seu saldo atual
    - Extrato com todas as suas transações
    - Listagem com todas as suas notificações

- Lojista deve poder acessar
    - Relatórios de transações, com filtros de datas, valores e clientes
    - Relatório de notificações, com pendentes, sucessos e erros

- Adm deve poder acessar
    - Tela de consistência financeira
    - Informações sobre Clientes e Lojistas
    - Status de todos os serviços
    - Filas, eventos e tasks
    - Reprocessamento de tasks com erros

- Rinha de backend
- Concorrência e consistência dos valores (lock)
- Documentação da API com o Scalar
- CI/CD com o GitHub Actions
- Deploy no Railway (picpay.zaqbit.com)
- Logs
- Observability
- Docker compose
- Testes de carga com K6
- Rate limit
- Diagrama do banco de dados
- Testes automatizados rodando cenários iguais aos de produção
- Report de cobertura de testes
- Testes de mutação
- Design Patterns
- Foco em segurança e consistência
- Nada hard-coded, tudo via configuração
- Auditoria de todas as operacoes

- Escala horizontal da API e do Worker (banco???)

- Feature de bônus ao convidar um amigo
    - Quem convidou ganha R$ 50
    - Quem foi convidado ganha R$ 100
    - Há um limite de 10 convites aceitos por cliente

## Conceitos e Decições

- Usei TDD para desenvolver a maioria do código
- Lock no banco de dados para concorrência e consistência
- Outbox Pattern
- Domain Events
- Result Pattern
- Vertical Slices Architecture

## Arquitetura

Utilizar modelo C4?
Ícones específicos e intuitivos para cada serviço/usuário/conceito

- Api
    - EF Core
    - 
- Worker
- Web
- Auth
- Notify
- Postgres

- Users
    - id
    - name
    - doc
    - email
    - password
    - created_at

- Wallets
    - id
    - user_id
    - type
    - balance
    - created_at

- Transactions
    - id
    - source_id
    - target_id
    - amount
    - created_at

- Notifications
    - id
    - user_id
    - message
    - status
    - created_at

## Casos de Uso

Casos de uso mapeados, facilitando a implementação e os testes.

### Cadastro do usuário Adm
- Deve ser feito direto no banco de dados
- O saldo dele deve ser 1.000.000.000.000
- A soma de todos os saldos SEMPRE VAI SER constante

### Cadastro de Clientes ou Lojistas
    - Validar se os documentos são válidos
    - Validar se o email é válido
    - Validar se já existe no banco (índice de unicidade no doc e no email)

### Transferência de dinheiro

Realizada via POST /transfers
{
    "amount": 15874,
    "target": "d25c18d5-5bf3-49fb-8d9c-67021738152f"
}

- Chamada não autenticada deve receber 403

- Apenas Clientes podem transferir
    - Lojista deve receber 401
    - Adm deve receber 401

- Não pode transferir valor <= zero
    - Validar amount enviado

- Não pode transferir para si próprio
    - Validar o target e o id do usuário logado

- Nâo pode transferir pra uma Carteira inexistente
    - Validar no banco se o target existe

- Não pode transferir sem saldo suficiente
    - Validar se tem saldo suficiente

- Não pode transferir caso seja não autorizado
    - Caso chamada pro Auth retorne não autorizado, retornar erro

- Não pode transferir caso o autorizador esteja fora do ar
    - Caso chamada pro Auth erro/timeout, retornar erro

- (lock) Não deve transferir em paralelo, gastando mais que o saldo


- (lock) Quem recebe duas ao mesmo tempo deve acabar com o saldo correto


- (lock) Quem enviar e recebe ao mesmo tempo deve acabar com o saldo correto


- Ao final, ambos devem ser notificados da transação

- Cliente pode transferir para um Lojista
    - Cliente possui R$ 100 de saldo
    - Lojista possui R$ 1400 de saldo
    - ACT -> Transferência de R$ 60
    - Cliente fica com R$ 40 e Lojista com R$ 1460
    - Transação + evento criados
    - Criar TestCases com vários valores diferentes

- Cliente pode transferir para outro Cliente
    - Cliente A possui R$ 100 de saldo
    - Cliente B possui R$ 1400 de saldo
    - ACT -> Transferência de R$ 60
    - Cliente fica com R$ 40 e Lojista com R$ 1460
    - Transação + evento criados
    - Criar TestCases com vários valores diferentes

### Extratos e consistência financeira

- Todas as transações são imutáveis
- Tudo deve ser salvo para fins de histórico e auditoria

## Testes

- Adicionar description nos testes, mostrar no log de execução
- Testes com as APIs externas fora do ar (timeout baixo)
- Testes de carga com o K6

- Uso do pattern Facade nos testes, abstraindo as chamadas pra API
- Para cada caso de uso, colocar um print ou code sniped do teste

- Auth e Notify devem se comportar dinâmicamente
    - Passo algum parâmetro na query string que altera o valor do retorno no endpoint
    - Uso de tabelas no banco?

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

