using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Reflection;

namespace MeuSuporte
{
    public partial class FormSelecaoUsuario : Form
    {
        public string Nome = "";

        public FormSelecaoUsuario()
        {
            InitializeComponent();
        }

        private void FormSelecaoUsuario_Load(object sender, EventArgs e)
        {
            carregaUsuarios();
        }

        void CarregaLista(string nome)
        {
            ListViewItem _item = new ListViewItem(nome);
           // _item.SubItems.Add(acesso);
            clientList.Items.Add(_item);
        }

        void carregaUsuarios()
        {
            DirectoryEntry machine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer");

            if (machine.Children != null)
            {
                var results = machine.Children.Cast<DirectoryEntry>().Where(r => r.SchemaClassName == "User").OrderBy(r => r.Name);

                foreach (DirectoryEntry child in results)
                {
                    if (child.Name != "WDAGUtilityAccount" && child.Name != "DefaultAccount")
                    {
                        CarregaLista(child.Name);                        
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Nome = clientList.FocusedItem.SubItems[0].Text;
            this.Close();
        }

        private void FormSelecaoUsuario_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Nome == "")
            {
                e.Cancel = true;
                MessageBox.Show("Selecione um Usuario");
            }
        }

    }
}
