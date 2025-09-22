using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Rechner
{
    public partial class Form1 : Form
    {
        // Verlauf (Historie) für die RichTextBox
        private string history = string.Empty;
        // Aktuelle Aufgabe für die TextBox1
        private string currentTask = string.Empty;
        // Zuletzt berechnetes Ergebnis für Fortsetzungen
        private string lastResult = string.Empty;
        // Nach '=' auf nächste Eingabe warten
        private bool awaitingPostEquals = false;

        public Form1()
        {
            InitializeComponent();
            // Keyboard: KeyPreview und Handler aktivieren
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.KeyPress += Form1_KeyPress;
            // Optional: Standard-Button auf '=' setzen
            this.AcceptButton = button13;

            // Event-Handler für alle relevanten Buttons
            button0.Click += Button_Click;
            button1.Click += Button_Click;
            button2.Click += Button_Click;
            button3.Click += Button_Click;
            button4.Click += Button_Click;
            button5.Click += Button_Click;
            button6.Click += Button_Click;
            button7.Click += Button_Click;
            button8.Click += Button_Click;
            button9.Click += Button_Click;
            button10.Click += Button_Click; // %
            button12.Click += Button_Click; // ,
            button14.Click += Button_Click; // +
            button15.Click += Button_Click; // -
            button16.Click += Button_Click; // *
            button17.Click += Button_Click; // /
            button18.Click += Button_Click; // C
            button21.Click += Button_Click; // ⌫
            button22.Click += Button_Click; // CE
            button19.Click += Button_Click; // √
            button20.Click += Button_Click; // xʸ
            button13.Click += ButtonEquals_Click; // =
        }

        private static bool IsOperator(string t)
        {
            return t == "+" || t == "-" || t == "*" || t == "/" || t == "×" || t == "÷" || t == "%";
        }

        private static bool IsDigitOrDecimal(string t)
        {
            if (t == "," || t == ".") return true;
            if (t.Length == 1 && char.IsDigit(t[0])) return true;
            return false;
        }

        private static bool IsUnaryOp(string t)
        {
            return t == "√" || t == "xʸ";
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            ProcessToken(btn.Text);
        }

        private void ProcessToken(string t)
        {
            // Steuerfunktionen auf currentTask anwenden
            switch (t)
            {
                case "CE":
                    ClearEntry();
                    awaitingPostEquals = false;
                    UpdateTextBox();
                    return;
                case "C":
                    currentTask = string.Empty;
                    awaitingPostEquals = false;
                    UpdateTextBox();
                    return;
                case "⌫":
                    if (!string.IsNullOrEmpty(currentTask))
                    {
                        currentTask = currentTask.Substring(0, currentTask.Length - 1);
                    }
                    awaitingPostEquals = false;
                    UpdateTextBox();
                    return;
                default:
                    break;
            }

            // Verhalten direkt nach '=': Anzeige bleibt bis zur nächsten Taste stehen
            if (awaitingPostEquals)
            {
                if (IsOperator(t) || IsUnaryOp(t))
                {
                    // Ergebnis übernehmen und mit gewähltem Operator/Unary weiterarbeiten
                    currentTask = lastResult + t;
                }
                else if (IsDigitOrDecimal(t))
                {
                    // Neue Zahl beginnen → Anzeige leeren und neu starten
                    currentTask = t == "." ? "," : t;
                }
                else
                {
                    // Sonstiges Zeichen (z. B. %) fällt unter Operatoren oben, falls nicht erfasst: anhängen
                    currentTask = t;
                }
                awaitingPostEquals = false;
                UpdateTextBox();
                return;
            }

            // Eingabezeichen an aktuelle Aufgabe anhängen
            if (t == ".") t = ","; // Punkt-Taste als Komma interpretieren
            currentTask += t;
            UpdateTextBox();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            string token = null;

            if (char.IsDigit(ch))
                token = ch.ToString();
            else
            {
                switch (ch)
                {
                    case '+': token = "+"; break;
                    case '-': token = "-"; break;
                    case '*': token = "*"; break;
                    case '/': token = "/"; break;
                    case ',': token = ","; break;
                    case '.': token = ","; break;
                    case '%': token = "%"; break;
                    case '=':
                        ButtonEquals_Click(this, EventArgs.Empty);
                        e.Handled = true; return;
                    case '\r':
                        ButtonEquals_Click(this, EventArgs.Empty);
                        e.Handled = true; return;
                    default:
                        break;
                }
            }

            if (token != null)
            {
                ProcessToken(token);
                e.Handled = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Map Keyboard/Numpad -> Tokens für nicht zeichenbasierte Tasten
            string token = null;
            bool handled = true;

            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && !e.Shift)
            {
                token = ((char)('0' + (e.KeyCode - Keys.D0))).ToString();
            }
            else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            {
                token = ((char)('0' + (e.KeyCode - Keys.NumPad0))).ToString();
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Add:
                        token = "+"; break;
                    case Keys.Subtract:
                        token = "-"; break;
                    case Keys.OemMinus:
                        token = "-"; break;
                    case Keys.Multiply:
                        token = "*"; break;
                    case Keys.Divide:
                        token = "/"; break;
                    case Keys.Oem2: // '/?'
                        token = "/"; break;
                    case Keys.Decimal:
                        token = ","; break;
                    case Keys.OemPeriod:
                        token = ","; break;
                    case Keys.Back:
                        token = "⌫"; break;
                    case Keys.Delete:
                        token = "CE"; break;
                    case Keys.Escape:
                        token = "C"; break;
                    case Keys.Enter:
                        ButtonEquals_Click(this, EventArgs.Empty);
                        e.SuppressKeyPress = true; e.Handled = true; return;
                    default:
                        handled = false; break;
                }
            }

            if (token != null)
            {
                ProcessToken(token);
            }

            if (handled || token != null)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void ButtonEquals_Click(object sender, EventArgs e)
        {
            // Nur die aktuelle Aufgabe berechnen
            string expression = currentTask;
            if (string.IsNullOrWhiteSpace(expression)) return;

            string normalized = NormalizeExpression(expression);
            try
            {
                // xʸ vorrechnen (als '^' markiert)
                normalized = EvaluatePowers(normalized);

                var result = new System.Data.DataTable().Compute(normalized, null);
                lastResult = result.ToString();

                // In die Historie übernehmen: Aufgabe = Ergebnis + Zeilenumbruch für die nächste Aufgabe
                history += expression + "=\n" + lastResult + "\n \n";

                // Aktuelle Aufgabe NICHT sofort löschen – bis zur nächsten Taste sichtbar lassen
                awaitingPostEquals = true;

                UpdateTextBox();
            }
            catch
            {
                // Optional: Fehler in currentTask anzeigen
                currentTask = "Fehler";
                UpdateTextBox();
                currentTask = string.Empty;
                lastResult = string.Empty;
                awaitingPostEquals = false;
            }
        }

        // CE: löscht die Zahl seit dem letzten Rechenzeichen in currentTask
        private void ClearEntry()
        {
            if (string.IsNullOrEmpty(currentTask))
            {
                return;
            }

            // Rechenoperatoren, die als Trenner gelten
            char[] ops = new[] { '+', '-', '*', '/', '×', '÷' };

            string work = currentTask.TrimEnd();
            int opIndex = work.LastIndexOfAny(ops);
            if (opIndex >= 0 && opIndex < work.Length - 1)
            {
                work = work.Substring(0, opIndex + 1);
            }
            else if (opIndex == work.Length - 1)
            {
                // Endet bereits mit Operator → nichts löschen
            }
            else
            {
                // Kein Operator gefunden → alles löschen
                work = string.Empty;
            }
            currentTask = work;
        }

        // Kontextabhängige Prozent-Expansion (unverändert)
        private static string ExpandPercent(string expr)
        {
            if (string.IsNullOrEmpty(expr)) return expr;

            var sb = new StringBuilder(expr);
            int i = 0;
            while (i < sb.Length)
            {
                if (sb[i] != '%') { i++; continue; }

                // Finde Token links von '%': Zahl, (....) oder negate(...)
                int tokenEnd = i - 1;
                int tokenStart;
                if (tokenEnd >= 0 && (char.IsDigit(sb[tokenEnd]) || sb[tokenEnd] == '.'))
                {
                    tokenStart = tokenEnd;
                    while (tokenStart >= 0 && (char.IsDigit(sb[tokenStart]) || sb[tokenStart] == '.')) tokenStart--;
                    tokenStart++;
                }
                else if (tokenEnd >= 0 && sb[tokenEnd] == ')')
                {
                    // Klammerausdruck zurückverfolgen
                    int depth = 0;
                    int k = tokenEnd;
                    while (k >= 0)
                    {
                        if (sb[k] == ')') depth++;
                        else if (sb[k] == '(') { depth--; if (depth == 0) break; }
                        k--;
                    }
                    if (k < 0) { i++; continue; }
                    tokenStart = k;
                    // Prüfen auf negate(
                    int maybeNegate = tokenStart - 6;
                    if (maybeNegate >= 0 && sb.ToString(maybeNegate, 6) == "negate")
                    {
                        tokenStart = maybeNegate;
                    }
                }
                else
                {
                    // Kein erkennbarer Token → einfacher /100
                    sb.Remove(i, 1);
                    sb.Insert(i, "/100");
                    i += 4;
                    continue;
                }

                string y = sb.ToString(tokenStart, tokenEnd - tokenStart + 1);

                // Operator vor dem Token finden
                int j = tokenStart - 1;
                while (j >= 0 && char.IsWhiteSpace(sb[j])) j--;
                char op = '\0';
                int opIndex = j;
                if (j >= 0 && (sb[j] == '+' || sb[j] == '-' || sb[j] == '*' || sb[j] == '/'))
                {
                    op = sb[j];
                }
                else
                {
                    opIndex = -1;
                }

                string replacement;
                if (op == '+' || op == '-')
                {
                    string baseExpr = sb.ToString(0, opIndex).Trim();
                    if (string.IsNullOrEmpty(baseExpr)) baseExpr = "0";
                    replacement = "(" + baseExpr + "/100*" + y + ")";
                }
                else if (op == '*' || op == '/')
                {
                    replacement = "(" + y + "/100)";
                }
                else
                {
                    replacement = "(" + y + "/100)";
                }

                // Ersetze Token% durch Replacement
                sb.Remove(tokenStart, i - tokenStart + 1);
                sb.Insert(tokenStart, replacement);
                i = tokenStart + replacement.Length;
            }
            return sb.ToString();
        }

        // negate(...) in eine gültige Rechenform bringen (unäres Minus)
        private static string ExpandNegate(string expr)
        {
            if (string.IsNullOrEmpty(expr)) return expr;
            return expr.Replace("negate(", "-(");
        }

        // Zentrale Normalisierung: mehrere Symbole für DataTable-Compute anpassen
        private static string NormalizeExpression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return string.Empty;

            // Alle Whitespaces und Zeilenumbrüche entfernen
            expression = expression.Replace(" ", string.Empty)
                                   .Replace("\r", string.Empty)
                                   .Replace("\n", string.Empty);
            expression = expression.Replace("×", "*");         // Multiplikation
            expression = expression.Replace("·", "*");         // Mittelpunkt
            expression = expression.Replace("÷", "/");         // Division
            expression = expression.Replace("−", "-");         // Unicode-Minus
            expression = expression.Replace(",", ".");         // Dezimal-Komma → Punkt

            // Prozent kontextabhängig expandieren (muss vor negate erfolgen)
            expression = ExpandPercent(expression);
            // negate(...) zu unärem Minus umschreiben
            expression = ExpandNegate(expression);

            // xʸ in '^' markieren
            expression = expression.Replace("xʸ", "^");

            return expression;
        }

        // '^'-Operatoren (aus xʸ) auswerten, rechtsassoziativ
        private static string EvaluatePowers(string expr)
        {
            if (string.IsNullOrEmpty(expr)) return expr;

            int FindMatchingOpen(string s, int closeIdx)
            {
                int depth = 0;
                for (int i = closeIdx; i >= 0; i--)
                {
                    if (s[i] == ')') depth++;
                    else if (s[i] == '(')
                    {
                        depth--;
                        if (depth == 0) return i;
                    }
                }
                return -1;
            }

            int FindMatchingClose(string s, int openIdx)
            {
                int depth = 0;
                for (int i = openIdx; i < s.Length; i++)
                {
                    if (s[i] == '(') depth++;
                    else if (s[i] == ')')
                    {
                        depth--;
                        if (depth == 0) return i;
                    }
                }
                return -1;
            }

            while (true)
            {
                int caret = expr.LastIndexOf('^');
                if (caret < 0) break;

                // Linken Operand finden
                int leftEnd = caret - 1;
                if (leftEnd < 0) break;
                int leftStart;
                if (expr[leftEnd] == ')')
                {
                    leftStart = FindMatchingOpen(expr, leftEnd);
                    if (leftStart < 0) throw new Exception("Klammerfehler");
                    // optionales vorangestelltes '-' vor '(...)' als unäres Minus berücksichtigen
                    if (leftStart - 1 >= 0 && expr[leftStart - 1] == '-')
                    {
                        // prüfen, ob unäres Minus (am Anfang oder nach Operator/()
                        int p = leftStart - 2;
                        if (p < 0 || "+-*/(^".IndexOf(expr[p]) >= 0)
                        {
                            leftStart -= 1;
                        }
                    }
                }
                else
                {
                    leftStart = leftEnd;
                    while (leftStart >= 0 && (char.IsDigit(expr[leftStart]) || expr[leftStart] == '.')) leftStart--;
                    // optionales vorangestelltes unäres '-'
                    if (leftStart >= 0 && expr[leftStart] == '-')
                    {
                        int p = leftStart - 1;
                        if (p < 0 || "+-*/(^".IndexOf(expr[p]) >= 0)
                        {
                            leftStart--;
                        }
                    }
                    leftStart++;
                }

                // Rechten Operand finden
                int rightStart = caret + 1;
                if (rightStart >= expr.Length) break;
                int rightEnd;
                if (expr[rightStart] == '(')
                {
                    rightEnd = FindMatchingClose(expr, rightStart);
                    if (rightEnd < 0) throw new Exception("Klammerfehler");
                    // optionales '-(expr)' wurde zu '-(expr)' bereits abgebildet; wenn ein vorangestelltes '-' ist, ist es an position rightStart-1
                }
                else if (expr[rightStart] == '-')
                {
                    int i = rightStart + 1;
                    if (i < expr.Length && expr[i] == '(')
                    {
                        rightEnd = FindMatchingClose(expr, i);
                    }
                    else
                    {
                        while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.')) i++;
                        rightEnd = i - 1;
                    }
                }
                else
                {
                    int i = rightStart;
                    while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.')) i++;
                    rightEnd = i - 1;
                }

                string leftExpr = expr.Substring(leftStart, leftEnd - leftStart + 1);
                string rightExpr = expr.Substring(rightStart, rightEnd - rightStart + 1);

                // Rekursiv interne '^' auswerten
                string leftReduced = EvaluatePowers(leftExpr);
                string rightReduced = EvaluatePowers(rightExpr);

                // Werte berechnen mit DataTable.Compute
                var dt = new System.Data.DataTable();
                object leftVal = dt.Compute(leftReduced, null);
                object rightVal = dt.Compute(rightReduced, null);

                double a = Convert.ToDouble(leftVal, CultureInfo.InvariantCulture);
                double b = Convert.ToDouble(rightVal, CultureInfo.InvariantCulture);
                double pow = Math.Pow(a, b);
                string powStr = pow.ToString("R", CultureInfo.InvariantCulture);

                // Ersetzen im Ausdruck
                expr = expr.Substring(0, leftStart) + powStr + expr.Substring(rightEnd + 1);
            }

            return expr;
        }

        private void UpdateTextBox()
        {
            // Anzeigen: TextBox1 zeigt aktuelle Aufgabe, bei awaitingPostEquals zusätzlich das Ergebnis
            if (awaitingPostEquals && !string.IsNullOrEmpty(currentTask) && !string.IsNullOrEmpty(lastResult))
            {
                textBox1.Text = currentTask + "=\r\n" + lastResult;
            }
            else
            {
                textBox1.Text = currentTask;
            }

            richTextBox1.SuspendLayout();
            richTextBox1.Text = history;
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
            richTextBox1.ResumeLayout();
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void button11_Click(object sender, EventArgs e)
        {
            // Nach einem Ergebnis: neue Eingabe mit negate(Ergebnis) beginnen
            if (awaitingPostEquals)
            {
                currentTask = $"negate({lastResult})";
                awaitingPostEquals = false;
                UpdateTextBox();
                return;
            }

            // +/- auf aktuelle Aufgabe anwenden
            if (string.IsNullOrEmpty(currentTask))
            {
                currentTask = "-"; // neue negative Zahl beginnen
                awaitingPostEquals = false;
                UpdateTextBox();
                return;
            }

            char[] ops = new[] { '+', '-', '*', '/', '×', '÷' };
            string segment = currentTask;
            int lastOp = segment.LastIndexOfAny(ops);
            int numStart = lastOp >= 0 ? lastOp + 1 : 0;

            string beforeNum = segment.Substring(0, numStart);
            string num = segment.Substring(numStart);
            if (num.Length == 0)
            {
                currentTask = beforeNum + "-"; // beginne negative Zahl
                awaitingPostEquals = false;
                UpdateTextBox();
                return;
            }

            // Prozent-Suffix berücksichtigen
            string suffix = string.Empty;
            string numCore = num;
            if (num.EndsWith("%"))
            {
                suffix = "%";
                numCore = num.Substring(0, num.Length - 1);
            }

            if (numCore.StartsWith("negate(") && numCore.EndsWith(")"))
            {
                string inner = numCore.Substring(7, numCore.Length - 8);
                numCore = inner; // Vorzeichen löschen
            }
            else if (numCore.StartsWith("-"))
            {
                // Negativ → Vorzeichen entfernen
                numCore = numCore.Substring(1);
            }
            else
            {
                // Positiv → negate() herumlegen
                numCore = $"negate({numCore})";
            }

            currentTask = beforeNum + numCore + suffix;
            awaitingPostEquals = false;
            UpdateTextBox();
        }
        private void button12_Click(object sender, EventArgs e) { }
        private void button15_Click(object sender, EventArgs e) { }
        private void button16_Click(object sender, EventArgs e) { }
        private void button17_Click(object sender, EventArgs e) { }
        private void button21_Click(object sender, EventArgs e) { }
        private void button0_Click(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void button18_Click(object sender, EventArgs e) { }
        private void button23_Click(object sender, EventArgs e)
        {
            // Rechner beenden
            this.Close();
            // Alternativ: Application.Exit();
        }
        private void button22_Click(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Enter global abfangen – auch wenn ein Button fokussiert ist
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Return)
            {
                ButtonEquals_Click(this, EventArgs.Empty);
                return true; // Ereignis verbraucht, kein Button-Click mehr
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
