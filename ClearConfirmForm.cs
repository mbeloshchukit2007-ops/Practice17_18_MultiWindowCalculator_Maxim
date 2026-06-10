using System;
using System.Drawing;
using System.Windows.Forms;

namespace Practice17_18_MultiWindowCalculator_Maxim;

public class ClearConfirmForm : Form
{
    public ClearConfirmForm()
    {
        Text = "Очистка стану калькулятора";
        Size = new Size(560, 170);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        Label question = new()
        {
            Text = "Ви дійсно бажаєте очистити стан калькулятора?",
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 13),
            ForeColor = Color.DarkOrange,
            Location = new Point(18, 18),
            Size = new Size(510, 48)
        };

        Button yes = new()
        {
            Text = "Так",
            Location = new Point(190, 84),
            Size = new Size(90, 30)
        };

        Button no = new()
        {
            Text = "Ні",
            Location = new Point(300, 84),
            Size = new Size(90, 30)
        };

        yes.Click += Yes_Click;
        no.Click += No_Click;

        AcceptButton = yes;
        CancelButton = no;

        Controls.Add(question);
        Controls.Add(yes);
        Controls.Add(no);
    }

    private void Yes_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Yes;
        Close();
    }

    private void No_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
        Close();
    }
}
