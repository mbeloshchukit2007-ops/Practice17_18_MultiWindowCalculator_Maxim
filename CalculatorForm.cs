using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Practice17_18_MultiWindowCalculator_Maxim;

public class CalculatorForm : Form
{
    private readonly Label screen = new();
    private readonly TableLayoutPanel buttonsPanel = new();
    private readonly RadioButton rbOn = new();
    private readonly RadioButton rbOff = new();

    private double firstValue;
    private string action = "";
    private bool nextNumber;

    public CalculatorForm()
    {
        Text = "Калькулятор";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(690, 550);
        MinimumSize = new Size(640, 520);
        BackColor = Color.FromArgb(244, 246, 248);
        KeyPreview = true;

        Panel topPanel = new()
        {
            Dock = DockStyle.Top,
            Height = 110,
            Padding = new Padding(14, 18, 14, 8)
        };

        screen.Text = "0";
        screen.Dock = DockStyle.Fill;
        screen.BackColor = Color.White;
        screen.ForeColor = Color.Black;
        screen.BorderStyle = BorderStyle.FixedSingle;
        screen.TextAlign = ContentAlignment.MiddleRight;
        screen.Font = new Font("Segoe UI", 26);
        topPanel.Controls.Add(screen);

        Panel statePanel = new()
        {
            Dock = DockStyle.Top,
            Height = 34,
            Padding = new Padding(14, 0, 14, 0)
        };

        rbOn.Text = "ON";
        rbOn.ForeColor = Color.SeaGreen;
        rbOn.AutoSize = true;
        rbOn.Checked = true;
        rbOn.Location = new Point(15, 7);

        rbOff.Text = "OFF";
        rbOff.ForeColor = Color.Crimson;
        rbOff.AutoSize = true;
        rbOff.Location = new Point(70, 7);

        statePanel.Controls.Add(rbOn);
        statePanel.Controls.Add(rbOff);

        buttonsPanel.Dock = DockStyle.Fill;
        buttonsPanel.ColumnCount = 4;
        buttonsPanel.RowCount = 5;
        buttonsPanel.Padding = new Padding(14);

        for (int i = 0; i < 4; i++)
        {
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        }

        for (int i = 0; i < 5; i++)
        {
            buttonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        }

        Button ce = AddButton("CE", 0, 0, ClearButton_Click);
        ce.ForeColor = Color.OrangeRed;

        AddButton("←", 1, 0, Backspace_Click);
        AddButton("%", 2, 0, Percent_Click);
        AddButton("/", 3, 0, Operation_Click);
        AddButton("7", 0, 1, Digit_Click);
        AddButton("8", 1, 1, Digit_Click);
        AddButton("9", 2, 1, Digit_Click);
        AddButton("*", 3, 1, Operation_Click);
        AddButton("4", 0, 2, Digit_Click);
        AddButton("5", 1, 2, Digit_Click);
        AddButton("6", 2, 2, Digit_Click);
        AddButton("-", 3, 2, Operation_Click);
        AddButton("1", 0, 3, Digit_Click);
        AddButton("2", 1, 3, Digit_Click);
        AddButton("3", 2, 3, Digit_Click);
        AddButton("+", 3, 3, Operation_Click);
        AddButton("0", 0, 4, Digit_Click);
        AddButton(".", 1, 4, Digit_Click);
        AddButton("=", 2, 4, Equal_Click, 2);

        Controls.Add(buttonsPanel);
        Controls.Add(statePanel);
        Controls.Add(topPanel);

        rbOn.CheckedChanged += StateRadio_CheckedChanged;
        rbOff.CheckedChanged += StateRadio_CheckedChanged;
        KeyDown += CalculatorForm_KeyDown;
    }

    private Button AddButton(string text, int column, int row, EventHandler click, int span = 1)
    {
        Button button = new()
        {
            Text = text,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 23),
            Margin = new Padding(5),
            BackColor = Color.WhiteSmoke,
            UseVisualStyleBackColor = true
        };

        button.Click += click;
        buttonsPanel.Controls.Add(button, column, row);

        if (span > 1)
        {
            buttonsPanel.SetColumnSpan(button, span);
        }

        return button;
    }

    private void ClearButton_Click(object? sender, EventArgs e)
    {
        using ClearConfirmForm dialog = new();
        DialogResult answer = dialog.ShowDialog(this);

        if (answer == DialogResult.Yes)
        {
            ClearCalculator();
        }
    }

    private void Digit_Click(object? sender, EventArgs e)
    {
        AddToScreen(((Button)sender!).Text);
    }

    private void AddToScreen(string symbol)
    {
        if (screen.Text == "0" || nextNumber)
        {
            screen.Text = "";
            nextNumber = false;
        }

        if (symbol == "." && screen.Text.Contains('.'))
        {
            return;
        }

        screen.Text += symbol;
    }

    private void Operation_Click(object? sender, EventArgs e)
    {
        firstValue = ReadNumber();
        action = ((Button)sender!).Text;
        nextNumber = true;
    }

    private void Equal_Click(object? sender, EventArgs e)
    {
        double secondValue = ReadNumber();
        double answer;

        if (action == "+")
        {
            answer = firstValue + secondValue;
        }
        else if (action == "-")
        {
            answer = firstValue - secondValue;
        }
        else if (action == "*")
        {
            answer = firstValue * secondValue;
        }
        else if (action == "/")
        {
            if (secondValue == 0)
            {
                MessageBox.Show("На нуль ділити не можна.", "Помилка");
                return;
            }

            answer = firstValue / secondValue;
        }
        else
        {
            return;
        }

        screen.Text = answer.ToString(CultureInfo.InvariantCulture);
        action = "";
        nextNumber = true;
    }

    private void Backspace_Click(object? sender, EventArgs e)
    {
        if (screen.Text.Length < 2)
        {
            screen.Text = "0";
        }
        else
        {
            screen.Text = screen.Text[..^1];
        }
    }

    private void Percent_Click(object? sender, EventArgs e)
    {
        screen.Text = (ReadNumber() / 100).ToString(CultureInfo.InvariantCulture);
        nextNumber = true;
    }

    private void ClearCalculator()
    {
        screen.Text = "0";
        firstValue = 0;
        action = "";
        nextNumber = false;
    }

    private void StateRadio_CheckedChanged(object? sender, EventArgs e)
    {
        bool enabled = rbOn.Checked;

        foreach (Control control in buttonsPanel.Controls)
        {
            control.Enabled = enabled;
        }

        screen.ForeColor = enabled ? Color.Black : Color.Gray;
    }

    private void CalculatorForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
        {
            AddToScreen((e.KeyCode - Keys.D0).ToString());
        }
        else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
        {
            AddToScreen((e.KeyCode - Keys.NumPad0).ToString());
        }
        else if (e.KeyCode == Keys.Add)
        {
            SelectOperation("+");
        }
        else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
        {
            SelectOperation("-");
        }
        else if (e.KeyCode == Keys.Multiply)
        {
            SelectOperation("*");
        }
        else if (e.KeyCode == Keys.Divide)
        {
            SelectOperation("/");
        }
        else if (e.KeyCode == Keys.Enter)
        {
            Equal_Click(this, EventArgs.Empty);
        }
        else if (e.KeyCode == Keys.Back)
        {
            Backspace_Click(this, EventArgs.Empty);
        }
    }

    private void SelectOperation(string value)
    {
        firstValue = ReadNumber();
        action = value;
        nextNumber = true;
    }

    private double ReadNumber()
    {
        if (double.TryParse(screen.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
        {
            return number;
        }

        return 0;
    }

    public void MakeScreens(string folder)
    {
        StartPosition = FormStartPosition.Manual;
        Location = new Point(70, 70);
        Show();
        Application.DoEvents();
        System.Threading.Thread.Sleep(300);

        SaveForm(Path.Combine(folder, "maxim_practice17_18_calculator.png"));

        using ClearConfirmForm dialog = new();
        dialog.StartPosition = FormStartPosition.Manual;
        dialog.Location = new Point(Location.X + 75, Location.Y + 205);
        dialog.Show(this);
        Application.DoEvents();
        System.Threading.Thread.Sleep(300);

        SaveControl(dialog, Path.Combine(folder, "maxim_practice17_18_dialog.png"));
        SaveScreenArea(Bounds, Path.Combine(folder, "maxim_practice17_18_dialog_on_form.png"));
        dialog.Close();
        Hide();
    }

    private void SaveForm(string path)
    {
        SaveScreenArea(Bounds, path);
    }

    private static void SaveControl(Control control, string path)
    {
        using Bitmap bitmap = new(control.Width, control.Height);
        control.DrawToBitmap(bitmap, new Rectangle(0, 0, control.Width, control.Height));
        bitmap.Save(path);
    }

    private void SaveWithDialog(Control dialog, Point position, string path)
    {
        using Bitmap formImage = new(Width, Height);
        DrawToBitmap(formImage, new Rectangle(0, 0, Width, Height));

        using Bitmap dialogImage = new(dialog.Width, dialog.Height);
        dialog.DrawToBitmap(dialogImage, new Rectangle(0, 0, dialog.Width, dialog.Height));

        using Graphics graphics = Graphics.FromImage(formImage);
        graphics.DrawImage(dialogImage, position);
        formImage.Save(path);
    }

    private static void SaveScreenArea(Rectangle area, string path)
    {
        using Bitmap bitmap = new(area.Width, area.Height);
        using Graphics graphics = Graphics.FromImage(bitmap);
        graphics.CopyFromScreen(area.Left, area.Top, 0, 0, area.Size);
        bitmap.Save(path);
    }
}
