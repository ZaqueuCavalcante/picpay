# TODOS

send notification to balzor wasm
https://github.com/dotnet/aspnetcore/discussions/54204
https://youtu.be/O7oaxFgNuYo


- Register
    - Customer
        - ✅ Validacao dos dados
        - ✅ Unicidade de email e cpf
    - Merchant
        - ✅ Validacao dos dados
        - ✅ Unicidade de email e cnpj

- Login
    - ✅ Adm, Customer and Merchant

- Customer
    - Olá, [UserName]
    - Saldo (olhinho de esconder)
    - Icone de notificacoes
        - Drawer exibindo todas
    - Na direita, icone de perfil
        - Drawer com nome, email, documento (formatado)
    - Transferencia
        - Informando WalletId + Valor
            - Caso tenha historico, exibir opcoes de carteiras
        - Ler QrCode (ja carrega tudo)
        - Acessar link de pagamento (tbm ja carrega tudo direto)
    - Extrato com todas as transacoes

- Merchant
    - Olá, [UserName]
    - Saldo (olhinho de esconder)
    - Icone de notificacoes
        - Drawer exibindo todas
    - Na direita, icone de perfil
        - Drawer com nome, email, documento (formatado)
    - Receber
        - Mostrar WalletId copiavel
        - Mostrar QrCode apenas para a chave
        - Mostrar QrCode para preencher chave + valor
        - Opcao de Link de Pagamento (WalletId + valor fixo)
    - Extrato com todas as transacoes

- Adm
    - Consistencia financeira








- SCANEAR QR CODE
- LINK DE PAGAMENTO (ESSENCIAL!!!!!)

- TESTES PARA OTIMIZAR PERFORMANCE DO ENDPOINT DE PAGAMENTO
    - TIRAR AS CONSTRAINTS DO BANCO

---------------------------------------

## Frontend

- Ratelimt na API, endpoint de registro ate 3 requests por hora

- /         -> Index (links para registar ou logar)
- /register -> Registrar
- /login    -> Login

- /         -> Index logado (Adm, Customer e Merchant)
    - Nome + Saldo
    - Transferir
    - Extrato
    - Notificacoes


- Varrer todos os arquivos, utilizar global usings




Transaction
    - Type      (Bônus de Boas-Vindas | Transferência recebida | Transferência enviada)
    - Amount    (R$ 5,87 | -R$ 9,14) 
    - Other     (PicPay | Customer | Merchant)
    - CreatedAt (dd/mm/AAAA 21:06)



