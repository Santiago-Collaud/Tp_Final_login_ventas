using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tp_Final_login_ventas
{
    public partial class Form_ventas : Form
    {
        public Form1 form_1 {  get; set; }

        public Form_ventas()
        {
            InitializeComponent();
            groupBox_A_Quien_Vendo.Visible = true;

            //invisibles
            groupBox_Seleccion_de_cliente.Visible = false;
            groupBox_Seleccion_de_Producto.Visible = false;
        }
        private void button_Venta_Clientes_Click(object sender, EventArgs e)
        {
            groupBox_Seleccion_de_cliente.Visible = true;
            dataGridView_Clientes_venta.DataSource = form_1.mostrar_Clientes();    
        }
        private void dataGridView_Clientes_venta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                label_Nombre_apellido_Selec_cliente.Text = dataGridView_dataGridView_Clientes_venta.CurrentRow.Cells[1].Value.ToString();
                textBox_Apellido_Modificar.Text = dataGridView_usuarios_registados.CurrentRow.Cells[2].Value.ToString();
                textBox_UserName_modificar.Text = dataGridView_usuarios_registados.CurrentRow.Cells[3].Value.ToString();
                textBox_Pass_Modificar.Text = dataGridView_usuarios_registados.CurrentRow.Cells[4].Value.ToString();
            }
            catch
            {

                MessageBox.Show("Upss... tuvimos un problema, intente de nuevo");
            }
        }

        private void button_Buscar_cliente_Click(object sender, EventArgs e)
        {
            
        }

        
    }
}
