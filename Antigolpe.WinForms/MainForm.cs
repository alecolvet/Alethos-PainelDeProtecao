using Antigolpe.Models;
using Antigolpe.Services;
using Antigolpe.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Antigolpe.WinForms;

public class MainForm : Form
{
    private readonly OcorrenciaService service = new();
    private readonly Color navy = Color.FromArgb(18, 29, 48);
    private readonly Color teal = Color.FromArgb(25, 169, 153);
    private readonly Color canvas = Color.FromArgb(244, 247, 250);
    private readonly Panel content = new();
    private DataGridView grid = new();
    private readonly Label pageTitle = new();
    private readonly Label pageSubtitle = new();
    private readonly TextBox searchBox = new();
    private readonly Label totalValue = new();
    private readonly Label pendingValue = new();
    private readonly Label highRiskValue = new();
    private string currentPage = "Dashboard";

    public MainForm()
    {
        Text = "Antigolpe | Painel de Proteção";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1050, 700);
        Size = new Size(1280, 820);
        BackColor = canvas;
        Font = new Font("Segoe UI", 10);
        BuildShell();
        ShowDashboard();
    }

    private void BuildShell()
    {
        var sidebar = new Panel { Dock = DockStyle.Left, Width = 235, BackColor = navy, Padding = new Padding(18, 22, 18, 18) };
        var brand = new Label { Text = "🛡  ANTIGOLPE", ForeColor = Color.White, Font = new Font("Segoe UI", 17, FontStyle.Bold), AutoSize = true, Location = new Point(18, 24) };
        var brandSub = new Label { Text = "CENTRAL DE PROTEÇÃO", ForeColor = Color.FromArgb(153, 173, 194), Font = new Font("Segoe UI", 8, FontStyle.Bold), AutoSize = true, Location = new Point(21, 58) };
        sidebar.Controls.Add(brand);
        sidebar.Controls.Add(brandSub);

        int y = 125;
        AddNav(sidebar, "▦    Visão geral", "Dashboard", ref y);
        AddNav(sidebar, "☷    Ocorrências", "Ocorrências", ref y);
        AddNav(sidebar, "＋    Nova ocorrência", "Nova", ref y);

        var footer = new Label
        {
            Text = "FIAP • CITIZENTECH\nProteção digital para todos",
            ForeColor = Color.FromArgb(140, 160, 180),
            Font = new Font("Segoe UI", 8),
            AutoSize = true,
            Location = new Point(20, 730),
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom
        };
        sidebar.Controls.Add(footer);

        var top = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = Color.White, Padding = new Padding(30, 16, 30, 12) };
        pageTitle.Font = new Font("Segoe UI", 19, FontStyle.Bold);
        pageTitle.ForeColor = navy;
        pageTitle.AutoSize = true;
        pageTitle.Location = new Point(28, 10);
        pageSubtitle.Font = new Font("Segoe UI", 9);
        pageSubtitle.ForeColor = Color.FromArgb(112, 126, 145);
        pageSubtitle.AutoSize = true;
        pageSubtitle.Location = new Point(30, 47);
        top.Controls.Add(pageTitle);
        top.Controls.Add(pageSubtitle);

        content.Dock = DockStyle.Fill;
        content.BackColor = canvas;
        content.Padding = new Padding(28, 24, 28, 24);

        Controls.Add(content);
        Controls.Add(top);
        Controls.Add(sidebar);
    }

    private void AddNav(Panel sidebar, string text, string page, ref int y)
    {
        var button = new Button
        {
            Text = text, Tag = page, Location = new Point(10, y), Size = new Size(195, 48),
            FlatStyle = FlatStyle.Flat, TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(220, 230, 240),
            BackColor = navy, Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 52, 75);
        button.Click += (_, _) =>
        {
            if (page == "Nova") { OpenCreateDialog(); return; }
            if (page == "Dashboard") ShowDashboard(); else ShowOccurrences();
        };
        sidebar.Controls.Add(button);
        y += 58;
    }

    private void SetPage(string title, string subtitle)
    {
        currentPage = title;
        pageTitle.Text = title;
        pageSubtitle.Text = subtitle;
        content.Controls.Clear();
    }

    private Panel Card(string title, Label value, string note, Color accent)
    {
        var panel = new Panel { BackColor = Color.White, Height = 120, Padding = new Padding(18), Margin = new Padding(0, 0, 14, 0) };
        var stripe = new Panel { BackColor = accent, Width = 4, Dock = DockStyle.Left };
        var t = new Label { Text = title.ToUpperInvariant(), Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(115, 130, 148), AutoSize = true, Location = new Point(18, 15) };
        value.Font = new Font("Segoe UI", 26, FontStyle.Bold);
        value.ForeColor = navy;
        value.AutoSize = true;
        value.Location = new Point(18, 35);
        var n = new Label { Text = note, Font = new Font("Segoe UI", 8), ForeColor = Color.FromArgb(130, 143, 158), AutoSize = true, Location = new Point(19, 88) };
        panel.Controls.Add(stripe); panel.Controls.Add(t); panel.Controls.Add(value); panel.Controls.Add(n);
        return panel;
    }

    private void ShowDashboard()
    {
        SetPage("Visão geral", "Acompanhe os registros e os principais indicadores de proteção.");
        var data = service.ListarTodas();
        totalValue.Text = data.Count.ToString();
        pendingValue.Text = data.Count(x => x.Status == "Pendente").ToString();
        highRiskValue.Text = data.Count(x => x.NivelRisco.Equals("Alto", StringComparison.OrdinalIgnoreCase)).ToString();

        var cards = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 145, WrapContents = false, BackColor = canvas };
        cards.Controls.Add(Card("Total de ocorrências", totalValue, "Registros no sistema", teal));
        cards.Controls.Add(Card("Pendentes", pendingValue, "Aguardando acompanhamento", Color.FromArgb(238, 164, 62)));
        cards.Controls.Add(Card("Alto risco", highRiskValue, "Precisam de atenção", Color.FromArgb(220, 88, 88)));
        content.Controls.Add(cards);

        var section = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = canvas, Padding = new Padding(0, 12, 0, 0) };
        var heading = new Label { Text = "Atividade recente", Font = new Font("Segoe UI", 13, FontStyle.Bold), ForeColor = navy, AutoSize = true, Location = new Point(0, 13) };
        var seeAll = ActionButton("Ver todas", false);
        seeAll.Size = new Size(110, 34); seeAll.Anchor = AnchorStyles.Top | AnchorStyles.Right; seeAll.Location = new Point(Width - 420, 7);
        seeAll.Click += (_, _) => ShowOccurrences();
        section.Controls.Add(heading); section.Controls.Add(seeAll);
        content.Controls.Add(section);

        ConfigureGrid();
        grid.DataSource = data.Take(8).ToList();
        content.Controls.Add(grid);
        grid.BringToFront();
        section.BringToFront();
        cards.BringToFront();
    }

    private void ShowOccurrences()
    {
        SetPage("Ocorrências", "Pesquise, acompanhe, atualize ou remova registros.");
        var toolbar = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = canvas };
        searchBox.Text = "";
        searchBox.PlaceholderText = "Pesquisar por mensagem, classificação ou status...";
        searchBox.Font = new Font("Segoe UI", 9);
        searchBox.Size = new Size(390, 36);
        searchBox.Location = new Point(0, 8);
        searchBox.TextChanged += (_, _) => LoadGrid();
        var add = ActionButton("+ Nova ocorrência", true);
        add.Size = new Size(170, 36); add.Location = new Point(405, 8);
        add.Click += (_, _) => OpenCreateDialog();
        var update = ActionButton("Alterar status", false);
        update.Size = new Size(130, 36); update.Location = new Point(585, 8);
        update.Click += (_, _) => UpdateSelected();
        var delete = ActionButton("Excluir", false);
        delete.Size = new Size(95, 36); delete.Location = new Point(725, 8);
        delete.ForeColor = Color.FromArgb(190, 65, 65);
        delete.Click += (_, _) => DeleteSelected();
        toolbar.Controls.AddRange(new Control[] { searchBox, add, update, delete });
        content.Controls.Add(toolbar);

        ConfigureGrid();
        grid.Dock = DockStyle.Fill;
        content.Controls.Add(grid);
        LoadGrid();
    }

    private void ConfigureGrid()
    {
        grid.DataSource = null;
        grid.Dock = DockStyle.Fill;
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.GridColor = Color.FromArgb(232, 237, 242);
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 251);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(85, 101, 120);
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        grid.DefaultCellStyle.ForeColor = navy;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(222, 243, 240);
        grid.DefaultCellStyle.SelectionForeColor = navy;
        grid.RowTemplate.Height = 42;
        grid.ColumnHeadersHeight = 42;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.RowHeadersVisible = false;
        grid.DataBindingComplete -= GridDataBindingComplete;
        grid.DataBindingComplete += GridDataBindingComplete;
    }

    private void GridDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        string[] hide = { "MotivoAnalise" };
        foreach (var col in hide)
        {
            if (grid.Columns[col] is { } column)
                column.Visible = false;
        }
        SetHeader("Id", "ID");
        SetHeader("Mensagem", "Mensagem analisada");
        SetHeader("ClassificacaoIA", "Classificação");
        SetHeader("NivelRisco", "Risco");
        SetHeader("Status", "Status");
        SetHeader("DataRegistro", "Data do registro");
        if (grid.Columns["Mensagem"] is { } mensagemColumn)
            mensagemColumn.FillWeight = 210;

        if (grid.Columns["DataRegistro"] is { } dataColumn)
            dataColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
    }

    private void SetHeader(string col, string header)
    {
        if (grid.Columns[col] is { } column)
            column.HeaderText = header;
    }

    private void LoadGrid()
    {
        var items = service.ListarTodas();
        string term = searchBox.Text.Trim();
        if (!string.IsNullOrWhiteSpace(term))
            items = items.Where(x =>
                x.Mensagem.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.ClassificacaoIA.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.Status.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                x.NivelRisco.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        grid.DataSource = items;
    }

    private Button ActionButton(string text, bool primary)
    {
        var b = new Button
        {
            Text = text, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
            BackColor = primary ? teal : Color.White,
            ForeColor = primary ? Color.White : navy,
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
        b.FlatAppearance.BorderSize = primary ? 0 : 1;
        b.FlatAppearance.BorderColor = Color.FromArgb(220, 227, 234);
        return b;
    }

    private void OpenCreateDialog()
    {
        using var dialog = new Form
        {
            Text = "Nova ocorrência", StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false,
            Size = new Size(590, 570), BackColor = Color.White, Font = Font
        };
        var title = new Label { Text = "Registrar ocorrência", Font = new Font("Segoe UI", 17, FontStyle.Bold), ForeColor = navy, Location = new Point(25, 20), AutoSize = true };
        var intro = new Label { Text = "Registre uma mensagem suspeita para acompanhamento.", ForeColor = Color.Gray, Location = new Point(27, 56), AutoSize = true };
        var msg = AddField(dialog, "Mensagem recebida", 95, true);
        var classification = AddCombo(dialog, "Classificação", 245, new[] { "Golpe", "Suspeito", "Seguro" });
        var risk = AddCombo(dialog, "Nível de risco", 315, new[] { "Alto", "Médio", "Baixo" });
        var reason = AddField(dialog, "Motivo da análise", 385, false);
        var cancel = ActionButton("Cancelar", false); cancel.SetBounds(315, 475, 100, 38); cancel.DialogResult = DialogResult.Cancel;
        var save = ActionButton("Salvar ocorrência", true); save.SetBounds(425, 475, 130, 38);
        save.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(msg.Text) || string.IsNullOrWhiteSpace(reason.Text))
            {
                MessageBox.Show(dialog, "Preencha a mensagem e o motivo da análise.", "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            service.Cadastrar(new Ocorrencia
            {
                Mensagem = msg.Text.Trim(),
                ClassificacaoIA = classification.Text,
                NivelRisco = risk.Text,
                MotivoAnalise = reason.Text.Trim(),
                Status = "Pendente",
                DataRegistro = DateTime.Now
            });
            dialog.DialogResult = DialogResult.OK;
            dialog.Close();
        };
        dialog.Controls.AddRange(new Control[] { title, intro, cancel, save });
        dialog.AcceptButton = save; dialog.CancelButton = cancel;
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            if (currentPage == "Ocorrências") LoadGrid(); else ShowDashboard();
        }

        TextBox AddField(Form f, string label, int y, bool multiline)
        {
            f.Controls.Add(new Label { Text = label, Location = new Point(27, y), AutoSize = true, ForeColor = navy, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
            var t = new TextBox { Location = new Point(27, y + 23), Width = 510, Height = multiline ? 105 : 30, Multiline = multiline, Font = new Font("Segoe UI", 9), BorderStyle = BorderStyle.FixedSingle };
            f.Controls.Add(t); return t;
        }
        ComboBox AddCombo(Form f, string label, int y, string[] options)
        {
            f.Controls.Add(new Label { Text = label, Location = new Point(27, y), AutoSize = true, ForeColor = navy, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
            var c = new ComboBox { Location = new Point(27, y + 23), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9) };
            c.Items.AddRange(options); c.SelectedIndex = 0; f.Controls.Add(c); return c;
        }
    }

    private int? SelectedId()
    {
        if (grid.CurrentRow?.DataBoundItem is Ocorrencia item) return item.Id;
        MessageBox.Show(this, "Selecione uma ocorrência na tabela.", "Nenhum registro selecionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }

    private void UpdateSelected()
    {
        var id = SelectedId(); if (id == null) return;
        using var dialog = new Form { Text = "Atualizar status", Size = new Size(360, 190), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, BackColor = Color.White };
        var combo = new ComboBox { Location = new Point(25, 30), Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };
        combo.Items.AddRange(new[] { "Pendente", "Em análise", "Resolvida" }); combo.SelectedIndex = 0;
        var save = ActionButton("Salvar", true); save.SetBounds(215, 85, 100, 36);
        save.Click += (_, _) => { service.AtualizarStatus(id.Value, combo.Text); dialog.DialogResult = DialogResult.OK; dialog.Close(); };
        dialog.Controls.AddRange(new Control[] { new Label { Text = "Novo status", Location = new Point(25, 10), AutoSize = true }, combo, save });
        if (dialog.ShowDialog(this) == DialogResult.OK) LoadGrid();
    }

    private void DeleteSelected()
    {
        var id = SelectedId(); if (id == null) return;
        if (MessageBox.Show(this, "Tem certeza de que deseja excluir esta ocorrência?", "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        {
            service.Excluir(id.Value);
            LoadGrid();
        }
    }
}
