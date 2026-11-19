
using Antigolpe.Models;
using Antigolpe.Services;

OcorrenciaService service = new OcorrenciaService();

bool executando = true;

while (executando)
{
    Console.Clear();

    Console.WriteLine("================================");
    Console.WriteLine("       ANTIGOLPE - FIAP");
    Console.WriteLine("================================");

    Console.WriteLine();
    Console.WriteLine("1 - Cadastrar ocorrência");
    Console.WriteLine("2 - Consultar ocorrências");
    Console.WriteLine("3 - Buscar ocorrência por ID");
    Console.WriteLine("4 - Atualizar status");
    Console.WriteLine("5 - Excluir ocorrência");
    Console.WriteLine("6 - Sair");

    Console.WriteLine();
    Console.Write("Escolha uma opção: ");

    string? opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            CadastrarOcorrencia();
            break;

        case "2":
            ListarOcorrencias();
            break;

        case "3":
            BuscarOcorrencia();
            break;

        case "4":
            AtualizarStatus();
            break;

        case "5":
            ExcluirOcorrencia();
            break;

        case "6":
            executando = false;
            Console.WriteLine("Encerrando o sistema...");
            break;

        default:
            Console.WriteLine("Opção inválida!");
            break;
    }

    if (executando)
    {
        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}

void CadastrarOcorrencia()
{
    Console.Clear();

    Console.WriteLine("=== CADASTRO DE OCORRÊNCIA ===");

    Console.Write("Mensagem suspeita: ");
    string mensagem = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(mensagem))
    {
        Console.WriteLine("A mensagem não pode ficar vazia.");
        return;
    }

    Console.Write("Classificação da IA (Golpe/Suspeito/Seguro): ");
    string classificacao = Console.ReadLine() ?? "";

    Console.Write("Nível de risco (Alto/Médio/Baixo): ");
    string risco = Console.ReadLine() ?? "";

    Console.Write("Motivo da análise: ");
    string motivo = Console.ReadLine() ?? "";

    Ocorrencia ocorrencia = new Ocorrencia
    {
        Mensagem = mensagem,
        ClassificacaoIA = classificacao,
        NivelRisco = risco,
        MotivoAnalise = motivo,
        Status = "Pendente"
    };

    service.Cadastrar(ocorrencia);

    Console.WriteLine();
    Console.WriteLine("Ocorrência cadastrada com sucesso!");
}

void ListarOcorrencias()
{
    Console.Clear();

    Console.WriteLine("=== OCORRÊNCIAS CADASTRADAS ===");

    List<Ocorrencia> ocorrencias = service.ListarTodas();

    if (ocorrencias.Count == 0)
    {
        Console.WriteLine("Nenhuma ocorrência encontrada.");
        return;
    }

    foreach (Ocorrencia ocorrencia in ocorrencias)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"ID: {ocorrencia.Id}");
        Console.WriteLine($"Mensagem: {ocorrencia.Mensagem}");
        Console.WriteLine($"Classificação: {ocorrencia.ClassificacaoIA}");
        Console.WriteLine($"Risco: {ocorrencia.NivelRisco}");
        Console.WriteLine($"Motivo: {ocorrencia.MotivoAnalise}");
        Console.WriteLine($"Status: {ocorrencia.Status}");
        Console.WriteLine($"Data: {ocorrencia.DataRegistro}");
    }

    Console.WriteLine("--------------------------------");
}

void BuscarOcorrencia()
{
    Console.Clear();

    Console.WriteLine("=== BUSCAR OCORRÊNCIA ===");

    Console.Write("Digite o ID: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    Ocorrencia? ocorrencia = service.BuscarPorId(id);

    if (ocorrencia == null)
    {
        Console.WriteLine("Ocorrência não encontrada.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"Mensagem: {ocorrencia.Mensagem}");
    Console.WriteLine($"Classificação: {ocorrencia.ClassificacaoIA}");
    Console.WriteLine($"Risco: {ocorrencia.NivelRisco}");
    Console.WriteLine($"Motivo: {ocorrencia.MotivoAnalise}");
    Console.WriteLine($"Status: {ocorrencia.Status}");
}


void AtualizarStatus()
{
    Console.Clear();

    Console.WriteLine("=== ATUALIZAR STATUS ===");

    Console.Write("ID da ocorrência: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    Console.WriteLine("1 - Pendente");
    Console.WriteLine("2 - Em análise");
    Console.WriteLine("3 - Resolvida");

    Console.Write("Novo status: ");

    string status = Console.ReadLine() switch
    {
        "1" => "Pendente",
        "2" => "Em análise",
        "3" => "Resolvida",
        _ => ""
    };

    if (status == "")
    {
        Console.WriteLine("Status inválido.");
        return;
    }

    bool atualizado = service.AtualizarStatus(id, status);

    Console.WriteLine(atualizado
        ? "Status atualizado com sucesso!"
        : "Ocorrência não encontrada.");
}

void ExcluirOcorrencia()
{
    Console.Clear();

    Console.WriteLine("=== EXCLUIR OCORRÊNCIA ===");

    Console.Write("ID da ocorrência: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    bool excluida = service.Excluir(id);

    Console.WriteLine(excluida
        ? "Ocorrência excluída com sucesso!"
        : "Ocorrência não encontrada.");
}
