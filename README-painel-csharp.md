# Alethos: Painel de Proteção (Windows Forms)

Painel desktop da CitizenTech **Alethos: guardião antigolpe**, projeto da Global Solution FIAP 2026.2.

## Equipe

**Nome da equipe:** [nome da equipe]

| Nome completo | RM |
| --- | --- |
| [Alexandre Colvet Delfino] | RM560059 |
| Enzo Luciano Barros De Oliveira | RM559557 |
| [Luigi Thiengo, nome completo] | RM560755 |
| [Pedro, nome completo] | RM561023 |
| [Witalon Rodrigues, nome completo] | RM559023 |

## O problema

Golpes digitais contra idosos crescem todo ano: falso parente pedindo Pix, falsa central do banco, links falsos e voz clonada por inteligência artificial. Em 2025 foram registrados 346.753 estelionatos virtuais no Brasil, quase 40 por hora (Anuário Brasileiro de Segurança Pública 2026). Muitos idosos recebem essas mensagens sozinhos e não têm a quem perguntar na hora.

## Pessoa real beneficiada

[Descreva a pessoa entrevistada pela equipe, sem dados que a identifiquem. Exemplo de formato: "Dona M., 74 anos, moradora da zona norte de São Paulo, recebeu uma mensagem de um falso neto pedindo Pix e quase transferiu o dinheiro."]

O painel também atende quem acompanha esses idosos: o atendente de um centro de convivência, CRAS ou associação de bairro, que registra os casos, acompanha o andamento e identifica os golpes mais frequentes para orientar as oficinas.

## A solução

O Painel de Proteção é a central do atendente. Nele é possível:

- ver um resumo com o total de ocorrências, as pendentes e as de alto risco;
- registrar uma mensagem suspeita, com a classificação, o nível de risco e o motivo da análise;
- pesquisar e listar as ocorrências;
- atualizar o status de cada caso e excluir registros.

## Como executar

Pré-requisitos: Windows 10 ou 11, Visual Studio 2026 (ou outra versão com suporte ao .NET 10) e o SDK do .NET 10.

1. Clone o repositório ou extraia o ZIP.
2. Abra `Antigolpe.slnx` no Visual Studio.
3. Clique com o botão direito em `Antigolpe.WinForms` e escolha **Definir como Projeto de Inicialização**.
4. Execute com `F5`.

Na primeira execução, o painel aplica as migrations e cria o banco SQLite em `%LOCALAPPDATA%\Antigolpe\antigolpe.db`. Use apenas dados fictícios nos testes.

## Tecnologias

- C# com .NET 10
- Windows Forms (interface)
- Entity Framework Core 10 com migrations
- SQLite (persistência local)

## Organização do código (POO)

| Pasta | Responsabilidade |
| --- | --- |
| `Antigolpe/Models` | Classe de domínio `Ocorrencia` |
| `Antigolpe/Services` | `OcorrenciaService`: cadastro, consulta, atualização de status e exclusão |
| `Antigolpe/Data` | `AppDbContext`: conexão com o SQLite |
| `Antigolpe/Migrations` | Histórico do schema do banco |
| `Antigolpe.WinForms` | Telas do painel (`MainForm`) |

## Versões

O projeto segue versionamento semântico (MAJOR.MINOR.PATCH). Cada mudança entra por pull request.

| Versão | O que mudou |
| --- | --- |
| v1.0.0 | Primeira versão do painel: cadastro, listagem, pesquisa, status e exclusão de ocorrências |

## Repositório

https://github.com/alecolvet/Antigolpe-PainelDeProtecao
