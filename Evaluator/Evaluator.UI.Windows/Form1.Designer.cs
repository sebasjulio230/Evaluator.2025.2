using Evaluator.Core;

namespace Evaluator.UI.Windows
{
    partial class Form1
    {
        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.TableLayoutPanel tableLayout;
        private System.Windows.Forms.Button btnClear, btnDelete, btnEqual;

        private void InitializeComponent()
        {
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // txtDisplay
            // 
            this.txtDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDisplay.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.txtDisplay.ReadOnly = true;
            this.txtDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDisplay.Height = 50;
            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 4;
            this.tableLayout.RowCount = 5;
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayout.ColumnStyles.Clear();
            for (int i = 0; i < 4; i++)
                this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));

            string[,] buttons = {
                {"7","8","9","/"},
                {"4","5","6","*"},
                {"1","2","3","-"},
                {"0",".","^","+"},
                {"(",")","Delete","Clear"}
            };

            for (int r = 0; r < buttons.GetLength(0); r++)
            {
                this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
                for (int c = 0; c < buttons.GetLength(1); c++)
                {
                    var text = buttons[r, c];
                    var btn = new System.Windows.Forms.Button();
                    btn.Text = text;
                    btn.Dock = System.Windows.Forms.DockStyle.Fill;
                    btn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
                    btn.Click += Button_Click;
                    this.tableLayout.Controls.Add(btn, c, r);
                }
            }

            // Botón '=' separado abajo
            this.btnEqual = new System.Windows.Forms.Button();
            this.btnEqual.Text = "=";
            this.btnEqual.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnEqual.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnEqual.Height = 50;
            this.btnEqual.Click += BtnEqual_Click;

            // Form1
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.Controls.Add(this.tableLayout);
            this.Controls.Add(this.btnEqual);
            this.Controls.Add(this.txtDisplay);
            this.Text = "Functions Evaluator";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            switch (btn.Text.Trim()) // Trim elimina espacios
            {
                case "Clear":
                    txtDisplay.Text = string.Empty;
                    break;
                case "Delete":
                    if (txtDisplay.Text.Length > 0)
                        txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
                    break;
                default:
                    txtDisplay.Text += btn.Text;
                    break;
            }
        }

        private void BtnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                var expr = txtDisplay.Text;
                var result = ExpressionEvaluator.Evaluate(expr);
                txtDisplay.Text = $"{expr}={result}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
