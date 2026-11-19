# Antigolpe — Windows Forms

## Grupo
Alexandre Colvet  - RM560059
Enzo Luciano      - RM559557
Luigi Thiengo     - RM560755
Pedro Claudino    - RM561023
Witalon Rodrigues - RM559023

## Abrir
1. Extraia a pasta `Antigolpe`.
2. Abra `Antigolpe.slnx` no Visual Studio 2022 (com suporte ao .NET 10).
3. Clique com o botão direito em `Antigolpe.WinForms` > **Definir como Projeto de Inicialização**.
4. Execute com `F5` ou `Ctrl+F5`.

## Banco de dados
O Console e o Windows Forms usam o mesmo arquivo:
`%LOCALAPPDATA%\\Antigolpe\\antigolpe.db`

Na inicialização, o Windows Forms aplica as migrations pendentes automaticamente. O banco existente na pasta `bin` do projeto Console não é movido automaticamente; se você já tinha registros nesse arquivo, copie `antigolpe.db` para a pasta indicada acima antes de executar o novo aplicativo.

## Funcionalidades
- Painel com total, pendentes e ocorrências de alto risco.
- Listagem e pesquisa de ocorrências.
- Cadastro, atualização de status e exclusão.
- Persistência com SQLite e Entity Framework Core.

## Observação acadêmica
A classificação e o nível de risco são informados manualmente nesta versão. Não há integração com um modelo de IA nesta interface. Use dados fictícios para demonstração e testes.
