using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tp_Final_login_ventas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //visibles
            groupBox_Login.Visible = true;

            //no visibles
            groupBox_Admin.Visible = false;
            groupBox_admin_usuario.Visible = false;
            groupBox_Eliminar_Usuario.Visible = false;
            groupBox_Crear_usuario.Visible = false;
            groupBox_Modificar.Visible = false;
            groupBox_Cliente.Visible = false;
            groupBox_Productos.Visible = false;
        }
        #region ADMIN USUARIOS
        public DataTable mostrar_usuarios()
        {
            Conexion.Conectar();
            DataTable dt = new DataTable();// crea el objeto data table
            string consulta = "SELECT * FROM Usuario";
            SqlCommand comando = new SqlCommand(consulta, Conexion.Conectar());

            SqlDataAdapter DA = new SqlDataAdapter(comando);
            DA.Fill(dt);
            return dt;

        }

        public bool buscar_usuario(string Nombre,string Nombre_Usuario)
        {
            Conexion.Conectar();
            string buscar_usuario = "SELECT * FROM Usuario WHERE Nombre =@nombre OR Nombre_Usuario=@nombre_usuario";
            SqlCommand cmd_buscar_usario = new SqlCommand(buscar_usuario,Conexion.Conectar());
            

            cmd_buscar_usario.Parameters.AddWithValue("@Nombre",Nombre);
            cmd_buscar_usario.Parameters.AddWithValue("@Nombre_Usuario", Nombre_Usuario);

            var cont = cmd_buscar_usario.ExecuteScalar();
            if(cont == null)
            {
                return true;// es prosible guardar
            }
            else
            {
                return false;//no es posible guardar
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button_Iniciar_Click(object sender, EventArgs e)
        {
            try
            {
                Conexion.Conectar();

                string login = "SELECT * FROM Usuario WHERE Nombre_Usuario COLLATE Latin1_General_BIN =@Nombre_Usuario AND pass=@pass";

                SqlCommand cmd_Log = new SqlCommand(login, Conexion.Conectar());
    
                cmd_Log.Parameters.AddWithValue("@Nombre_Usuario", textBox_Usuario_IN.Text);
                cmd_Log.Parameters.AddWithValue("@pass", textBox_Pass_IN.Text);

                var cont = cmd_Log.ExecuteScalar();
                if(cont==null)
                {
                    MessageBox.Show("usuario no encontrado");
                }
                else if ((bool)(cont = true))
                {
                    MessageBox.Show("Bienvenido");
                    groupBox_Admin.Visible = true;
                    groupBox_Login.Visible = false;
                }
                textBox_Pass_IN.Clear();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upsss \nOcurrió un error \nIntente de nuevo ");
            }
        }


        private void button_Salir_Click(object sender, EventArgs e)// boton salir
        {
            groupBox_Admin.Visible = false;
            groupBox_Login.Visible = true;
        }

        private void button_Admin_Click(object sender, EventArgs e)
        {
            groupBox_admin_usuario.Visible = true;
            groupBox_Crear_usuario.Visible = false;
            Conexion.Conectar();
            dataGridView_usuarios_registados.DataSource = mostrar_usuarios();

            //invisibles
            
        }

        private void button_Crear_Usuario_Click(object sender, EventArgs e)
        {
            groupBox_Crear_usuario.Visible = true;
        }

        private void button_Atras_Click(object sender, EventArgs e)
        {
            groupBox_Crear_usuario.Visible = false;

            textBox_Nombre_Crear.Clear();
            textBox_Apellido_Crear.Clear();
            textBox_UserName_Crear.Clear();
            textBox_Pass_Crear.Clear();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Conexion.Conectar();
            //ID,Nombre,Apellido,Nombre_Usuario,pass
            string agregar_usuario = "INSERT INTO Usuario (Nombre,Apellido,Nombre_Usuario,pass)VALUES(@nombre,@apellido,@nombre_usuario,@pass)";
            SqlCommand cmd_agregar = new SqlCommand(agregar_usuario, Conexion.Conectar());

            if(buscar_usuario(textBox_Nombre_Crear.Text, textBox_UserName_Crear.Text)==true)
            {
                cmd_agregar.Parameters.AddWithValue("@nombre", textBox_Nombre_Crear.Text);
                cmd_agregar.Parameters.AddWithValue("@apellido", textBox_Apellido_Crear.Text);
                cmd_agregar.Parameters.AddWithValue("@nombre_usuario", textBox_UserName_Crear.Text);
                cmd_agregar.Parameters.AddWithValue("@pass", textBox_Pass_Crear.Text);

                cmd_agregar.ExecuteNonQuery();
                MessageBox.Show("Usuario Creado con Exito");

                dataGridView_usuarios_registados.DataSource = mostrar_usuarios();
            }
           else
            {
                MessageBox.Show("El Usuario ya existe");
            }
            

            textBox_Nombre_Crear.Clear();
            textBox_Apellido_Crear.Clear();
            textBox_UserName_Crear.Clear();
            textBox_Pass_Crear.Clear();
        }

        private void button_Eliminar_Usuario_Click(object sender, EventArgs e)
        {
            groupBox_Eliminar_Usuario.Visible = true;
                
        }
        string _nombre_usuario;
        private void button_Buscar_Click(object sender, EventArgs e)
        {
            button_Confirmar_Eliminar.Enabled = false;
            try
            {
                Conexion.Conectar();
                string buscar = "SELECT * FROM usuario WHERE Nombre_Usuario COLLATE Latin1_General_BIN =@Nombre_Usuario";
                
                SqlCommand cmd_buscar = new SqlCommand(buscar, Conexion.Conectar());

                
                cmd_buscar.Parameters.AddWithValue("@Nombre_Usuario", textBox_Buscar.Text);

                var cont = cmd_buscar.ExecuteScalar();
                if (cont == null)
                {
                    MessageBox.Show("usuario no encontrado");
                }
                else if ((bool)(cont = true))
                {
                    SqlDataReader reader = cmd_buscar.ExecuteReader();
                    if (reader.Read())
                    {
                        string nombre = reader["Nombre"].ToString();
                        string apellido= reader["apellido"].ToString();
                        _nombre_usuario = reader["nombre_usuario"].ToString();
                        label_Nombre_Eliminar.Text = nombre;
                        label_Apellido_Eliminar.Text = apellido;
                        label_Usuario_Eliminar.Text = _nombre_usuario;
                    }
                    
                    
                    button_Confirmar_Eliminar.Enabled = true;

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upsss \nOcurrió un error \nIntente de nuevo ");
            }
            dataGridView_usuarios_registados.DataSource = mostrar_usuarios();
        }

        private void button_Confirmar_Eliminar_Click(object sender, EventArgs e)
        {
            Conexion.Conectar();
            string borrar = "DELETE FROM usuario WHERE Nombre_Usuario=@Nombre_Usuario";
            SqlCommand cmd_borrar = new SqlCommand(borrar, Conexion.Conectar());
            cmd_borrar.Parameters.AddWithValue("@Nombre_Usuario", _nombre_usuario);

            cmd_borrar.ExecuteNonQuery();
            dataGridView_usuarios_registados.DataSource= mostrar_usuarios();

            textBox_Buscar.Clear();
        }

        private void button_Atras_2_Click(object sender, EventArgs e)
        {
            groupBox_Eliminar_Usuario.Visible = false;
            textBox_Buscar.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox_admin_usuario.Visible = false;
        }

        private void button_Actualizar_Click(object sender, EventArgs e)
        {
            groupBox_Modificar.Visible = true;
        }
        private void dataGridView_usuarios_registados_CellContentClick_1(object sender, DataGridViewCellEventArgs e) //abrimos el majejador de eventos en las promiedades (reyito) y buscamos contentclick y hacemos doble click
        {//con un doble click toma el valor de la fila para modificar
            try
            {

                textBox_Nombre_Modificar.Text = dataGridView_usuarios_registados.CurrentRow.Cells[1].Value.ToString();
                textBox_Apellido_Modificar.Text = dataGridView_usuarios_registados.CurrentRow.Cells[2].Value.ToString();
                textBox_UserName_modificar.Text = dataGridView_usuarios_registados.CurrentRow.Cells[3].Value.ToString();
                textBox_Pass_Modificar.Text = dataGridView_usuarios_registados.CurrentRow.Cells[4].Value.ToString();
            }
            catch
            {

                MessageBox.Show("Upss... tuvimos un problema, intente de nuevo");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Conexion.Conectar();
            string modificar = "UPDATE Usuario SET Nombre=@nombre,apellido=@Apellido,nombre_usuario=@Nombre_Usuario,Pass=@pass WHERE ID=@id";
            SqlCommand cmd_modificar = new SqlCommand(modificar, Conexion.Conectar());

            cmd_modificar.Parameters.AddWithValue("@nombre", textBox_Nombre_Modificar.Text);
            cmd_modificar.Parameters.AddWithValue("@apellido", textBox_Apellido_Modificar.Text);
            cmd_modificar.Parameters.AddWithValue("@nombre_usuario", textBox_UserName_modificar.Text);
            cmd_modificar.Parameters.AddWithValue("@pass", textBox_Pass_Modificar.Text);
            cmd_modificar.Parameters.AddWithValue("@id", dataGridView_usuarios_registados.CurrentRow.Cells[0].Value);

            cmd_modificar.ExecuteNonQuery();

            dataGridView_usuarios_registados.DataSource = mostrar_usuarios();

            textBox_Nombre_Modificar.Clear();
            textBox_Apellido_Modificar.Clear();
            textBox_UserName_modificar.Clear();
            textBox_Pass_Modificar.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox_Modificar.Visible = false;
            textBox_Nombre_Modificar.Clear();
            textBox_Apellido_Modificar.Clear();
            textBox_UserName_modificar.Clear();
            textBox_Pass_Modificar.Clear();
        }
        #endregion

        #region ADMIN CLIENTES

        public DataTable mostrar_Clientes()
        {
            Conexion.Conectar();
            DataTable dt = new DataTable();// crea el objeto data table
            string consulta = "SELECT * FROM Cliente";
            SqlCommand comando = new SqlCommand(consulta, Conexion.Conectar());

            SqlDataAdapter DA = new SqlDataAdapter(comando);
            DA.Fill(dt);
            return dt;

        }
        public bool buscar_cliente(string Nombre, string Apellido,string DNI)
        {
            Conexion.Conectar();
            string buscar_usuario = "SELECT * FROM Cliente WHERE Nombre =@nombre OR Apellido=@Apellido OR DNI=@DNI";
            SqlCommand cmd_buscar_usario = new SqlCommand(buscar_usuario, Conexion.Conectar());


            cmd_buscar_usario.Parameters.AddWithValue("@Nombre", Nombre);
            cmd_buscar_usario.Parameters.AddWithValue("@Apellido", Apellido);
            cmd_buscar_usario.Parameters.AddWithValue("@DNI", DNI);

            var cont = cmd_buscar_usario.ExecuteScalar();
            if (cont == null)
            {
                return true;// es prosible guardar
            }
            else
            {
                return false;//no es posible guardar
            }

        }
        private void button_Clientes_Click(object sender, EventArgs e) 
        {
            groupBox_Cliente.Visible = true;
            dataGridView_Clientes.DataSource = mostrar_Clientes();
            groupBox_Informacion_clientes.Visible = false;
        }


        private void dataGridView_Clientes_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                textBox_Nombre_Cliente_IN.Text = dataGridView_Clientes.CurrentRow.Cells[1].Value.ToString();
                textBox_Apellido_Cliente_IN.Text = dataGridView_Clientes.CurrentRow.Cells[2].Value.ToString();
                textBoxDNI_Cliente_IN.Text = dataGridView_Clientes.CurrentRow.Cells[3].Value.ToString();
                textBox_Tel_Cliente_IN.Text = dataGridView_Clientes.CurrentRow.Cells[4].Value.ToString();
                textBox_Mail_Cliente_IN.Text = dataGridView_Clientes.CurrentRow.Cells[5].Value.ToString();

                DateTime fechaNacimiento = (DateTime)dataGridView_Clientes.CurrentRow.Cells[6].Value;
                textBox_Dia_cliente_IN.Text = fechaNacimiento.Day.ToString();
                textBox_Mes_Cliente_IN.Text = fechaNacimiento.Month.ToString();
                textBox_Año_Cliente_IN.Text = fechaNacimiento.Year.ToString();

                textBox_Dir_cliente_IN.Text= dataGridView_Clientes.CurrentRow.Cells[8].Value.ToString();
                

            }
            catch { }
        }
        private void button_Administrar_clientes_Click(object sender, EventArgs e)//debe mostrar la info del cliente
        {
            groupBox_Informacion_clientes.Visible = true;
        }
        private void button_modificar_cliente_Click(object sender, EventArgs e) //modifica el cliente con los datos que se trajeron de la tabla
        {
            Conexion.Conectar();
            string Modificar_Cliente= "UPDATE Cliente SET nombre=@Nombre,apellido=@Apellido,telefono=@Telefono,mail=@mail,DNI=@dni,fecha_nacimiento=@Fecha_Nacimiento, direccion=@Direccion WHERE ID=@id";
            SqlCommand cmd_Modificar_cliente = new SqlCommand(Modificar_Cliente, Conexion.Conectar());

            cmd_Modificar_cliente.Parameters.AddWithValue("@ID", dataGridView_Clientes.CurrentRow.Cells[0].Value);
            cmd_Modificar_cliente.Parameters.AddWithValue("@nombre",textBox_Nombre_Cliente_IN.Text);
            cmd_Modificar_cliente.Parameters.AddWithValue("@apellido", textBox_Apellido_Cliente_IN.Text);
            cmd_Modificar_cliente.Parameters.AddWithValue("@telefono", textBox_Tel_Cliente_IN.Text);
            cmd_Modificar_cliente.Parameters.AddWithValue("@mail", textBox_Mail_Cliente_IN.Text);
            cmd_Modificar_cliente.Parameters.AddWithValue("@dni", textBoxDNI_Cliente_IN.Text);
            
            int dia = int.Parse(textBox_Dia_cliente_IN.Text);
            int mes = int.Parse(textBox_Mes_Cliente_IN.Text);
            int año = int.Parse(textBox_Año_Cliente_IN.Text);
            DateTime fechaNacimiento = new DateTime(año, mes, dia);//creamo el objeto dateTime

            cmd_Modificar_cliente.Parameters.AddWithValue("@fecha_nacimiento", fechaNacimiento);
            
            cmd_Modificar_cliente.Parameters.AddWithValue("@direccion", textBox_Dir_cliente_IN.Text);
            


            cmd_Modificar_cliente.ExecuteNonQuery();

            MessageBox.Show("Modificaciones correctas");

            dataGridView_Clientes.DataSource = mostrar_Clientes();

            textBox_Nombre_Cliente_IN.Clear();
            textBox_Apellido_Cliente_IN.Clear();
            textBoxDNI_Cliente_IN.Clear();
            textBox_Tel_Cliente_IN.Clear();
            textBox_Dir_cliente_IN.Clear();
            textBox_Mail_Cliente_IN.Clear();
            textBox_Dia_cliente_IN.Clear();
            textBox_Mes_Cliente_IN.Clear();
            textBox_Año_Cliente_IN.Clear();

        }
        private void button_Crear_Cliente_Click(object sender, EventArgs e)
        {

            Conexion.Conectar();
            string agregar_cliente = "INSERT INTO Cliente (Nombre,Apellido,DNI,Telefono,mail,Fecha_Nacimiento,Direccion) VALUES (@nombre,@apellido,@dni,@telefono,@mail,@fecha_Nacimiento,@direccion)";
            SqlCommand cmd_Agregar_Cliente = new SqlCommand(agregar_cliente, Conexion.Conectar());

            if(buscar_cliente(textBox_Nombre_Cliente_IN.Text, textBox_Apellido_Cliente_IN.Text, textBoxDNI_Cliente_IN.Text)==true)
            {
                cmd_Agregar_Cliente.Parameters.AddWithValue("@nombre", textBox_Nombre_Cliente_IN.Text);
                cmd_Agregar_Cliente.Parameters.AddWithValue("@apellido", textBox_Apellido_Cliente_IN.Text);
                cmd_Agregar_Cliente.Parameters.AddWithValue("@dni", textBoxDNI_Cliente_IN.Text);
                cmd_Agregar_Cliente.Parameters.AddWithValue("@telefono", textBox_Tel_Cliente_IN.Text);
                cmd_Agregar_Cliente.Parameters.AddWithValue("@mail", textBox_Mail_Cliente_IN.Text);

                int dia = int.Parse(textBox_Dia_cliente_IN.Text);
                int mes = int.Parse(textBox_Mes_Cliente_IN.Text);
                int año = int.Parse(textBox_Año_Cliente_IN.Text);

                if((dia<0 || dia>31) ||(mes<0 || mes > 12) || (año<0 || año >2023))//verificamos fecha
                {
                    MessageBox.Show("La Fecha no es correcta");
                }
                else
                { 
                DateTime fechaNacimiento = new DateTime(año, mes, dia);//creamo el objeto dateTime

                cmd_Agregar_Cliente.Parameters.AddWithValue("@fecha_nacimiento", fechaNacimiento);
                cmd_Agregar_Cliente.Parameters.AddWithValue("@direccion", textBox_Dir_cliente_IN.Text);

                cmd_Agregar_Cliente.ExecuteNonQuery();

                MessageBox.Show("Exito");

                    textBox_Nombre_Cliente_IN.Clear();
                    textBox_Apellido_Cliente_IN.Clear();
                    textBoxDNI_Cliente_IN.Clear();
                    textBox_Tel_Cliente_IN.Clear();
                    textBox_Dir_cliente_IN.Clear();
                    textBox_Mail_Cliente_IN.Clear();
                    textBox_Dia_cliente_IN.Clear();
                    textBox_Mes_Cliente_IN.Clear();
                    textBox_Año_Cliente_IN.Clear();
                }
                dataGridView_Clientes.DataSource = mostrar_Clientes();
            }
            else
            {
                MessageBox.Show("El cliente ya existe");
            }
            

            
        }
        private void button_Borrar_Cliente_Click(object sender, EventArgs e)
        {
            Conexion.Conectar();
            string borrar_cliente = "DELETE FROM Cliente WHERE ID=@id";
            SqlCommand cmd_borrar_cliente = new SqlCommand(borrar_cliente, Conexion.Conectar());

            if (textBox_Nombre_Cliente_IN.Text != string.Empty && textBox_Apellido_Cliente_IN.Text != string.Empty)
            {
                cmd_borrar_cliente.Parameters.AddWithValue("@id", dataGridView_Clientes.CurrentRow.Cells[0].Value);

                cmd_borrar_cliente.ExecuteNonQuery();

                MessageBox.Show("Cliente eliminado");

                dataGridView_Clientes.DataSource = mostrar_Clientes();
            }
            else
            {
                MessageBox.Show("No se puede eliminar cliente\nControlar campos");
            }
            textBox_Nombre_Cliente_IN.Clear();
            textBox_Apellido_Cliente_IN.Clear();
            textBoxDNI_Cliente_IN.Clear();
            textBox_Tel_Cliente_IN.Clear();
            textBox_Dir_cliente_IN.Clear();
            textBox_Mail_Cliente_IN.Clear();
            textBox_Dia_cliente_IN.Clear();
            textBox_Mes_Cliente_IN.Clear();
            textBox_Año_Cliente_IN.Clear();

        }
        private void button_Atras_Info_Cliente_Click(object sender, EventArgs e)
        {
            groupBox_Informacion_clientes.Visible = false;

            textBox_Nombre_Cliente_IN.Clear();
            textBox_Apellido_Cliente_IN.Clear();
            textBoxDNI_Cliente_IN.Clear();
            textBox_Tel_Cliente_IN.Clear();
            textBox_Dir_cliente_IN.Clear();
            textBox_Mail_Cliente_IN.Clear();
            textBox_Dia_cliente_IN.Clear();
            textBox_Mes_Cliente_IN.Clear();
            textBox_Año_Cliente_IN.Clear();
        }

        private void button_Atras_Clientes_Click(object sender, EventArgs e)
        {
            groupBox_Cliente.Visible = false;
        }


        #endregion

        #region PRODUCTOS

        public DataTable mostrar_productos()
        {
            Conexion.Conectar();
            DataTable dt = new DataTable();// crea el objeto data table
            string consulta = "SELECT * FROM productos";
            SqlCommand comando = new SqlCommand(consulta, Conexion.Conectar());

            SqlDataAdapter DA = new SqlDataAdapter(comando);
            DA.Fill(dt);
            return dt;
        }
        public bool buscar_producto(string producto)
        {
            Conexion.Conectar();
            string buscar_usuario = "SELECT * FROM Productos WHERE Producto =@producto";
            SqlCommand cmd_buscar_usario = new SqlCommand(buscar_usuario, Conexion.Conectar());


            cmd_buscar_usario.Parameters.AddWithValue("@Producto", producto);
            

            var cont = cmd_buscar_usario.ExecuteScalar();
            if (cont == null)
            {
                return true;// es prosible guardar
            }
            else
            {
                return false;//no es posible guardar
            }

        }
            private void button_Productos_Click(object sender, EventArgs e)
        {
            groupBox_Productos.Visible = true;
            dataGridView_productos.DataSource = mostrar_productos();
            groupBox_Info_Producto.Visible = false;
        }
        private void button_Administrar_Producto_Click(object sender, EventArgs e)
        {
            groupBox_Info_Producto.Visible = true;
        }
        private void dataGridView_productos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox_Nombre_Producto_IN.Text = dataGridView_productos.CurrentRow.Cells[1].Value.ToString();
                textBox_Precio_Producto_IN.Text = dataGridView_productos.CurrentRow.Cells[2].Value.ToString();
                textBox_cantidad_productos_IN.Text = dataGridView_productos.CurrentRow.Cells[3].Value.ToString();              
            }
            catch (Exception)
            {

                
            }
        }
        private void button_Crear_Producto_Click(object sender, EventArgs e)
        {
            Conexion.Conectar();
            string crear_producto = "INSERT INTO Productos (Producto,Precio,Cantidad) VALUES(@producto,@precio,@cantidad)";
            SqlCommand cmd_crear_producto = new SqlCommand(crear_producto, Conexion.Conectar());

            if(buscar_producto(textBox_Nombre_Producto_IN.Text)==true)
            {
                cmd_crear_producto.Parameters.AddWithValue("@producto", textBox_Nombre_Producto_IN.Text);
                cmd_crear_producto.Parameters.AddWithValue("@precio", float.Parse(textBox_Precio_Producto_IN.Text));
                cmd_crear_producto.Parameters.AddWithValue("@cantidad", int.Parse(textBox_cantidad_productos_IN.Text));

                cmd_crear_producto.ExecuteNonQuery();
                MessageBox.Show("Producto creado");

                textBox_Nombre_Producto_IN.Clear();
                textBox_cantidad_productos_IN.Clear();
                textBox_Precio_Producto_IN.Clear();
            }
            else 
            {
                MessageBox.Show("El producto ya existe");
            }
            
            dataGridView_productos.DataSource = mostrar_productos();

           
        }

        private void button_MOD_producto_Click(object sender, EventArgs e)
        {
            try
            {
                Conexion.Conectar();
                string mod_producto = "UPDATE Productos SET producto=@Producto,cantidad=@Cantidad,precio=@Precio WHERE ID=@id";
                SqlCommand cmd_mod_producto = new SqlCommand(mod_producto, Conexion.Conectar());

                cmd_mod_producto.Parameters.AddWithValue("@ID", dataGridView_productos.CurrentRow.Cells[0].Value);
                cmd_mod_producto.Parameters.AddWithValue("@producto", textBox_Nombre_Producto_IN.Text);
                cmd_mod_producto.Parameters.AddWithValue("@cantidad", int.Parse(textBox_cantidad_productos_IN.Text));
                cmd_mod_producto.Parameters.AddWithValue("@precio", float.Parse(textBox_Precio_Producto_IN.Text));

                cmd_mod_producto.ExecuteNonQuery();

                MessageBox.Show("Producto creado con exito");

                dataGridView_productos.DataSource = mostrar_productos();

                textBox_Nombre_Producto_IN.Clear();
                textBox_cantidad_productos_IN.Clear();
                textBox_Precio_Producto_IN.Clear();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error en el acceso a la base de datos");
            }
        }
        private void button_Borrar_Producto_Click(object sender, EventArgs e)
        {
            Conexion.Conectar();
            string borrar_producto = "DELETE FROM Productos WHERE id=@id";
            SqlCommand cmd_borrar_producto= new SqlCommand(borrar_producto,Conexion.Conectar());

            cmd_borrar_producto.Parameters.AddWithValue("@id", dataGridView_productos.CurrentRow.Cells[0].Value);

            cmd_borrar_producto.ExecuteNonQuery();
            MessageBox.Show("Producto eliminado con exito");
            dataGridView_productos.DataSource = mostrar_productos();

            textBox_Nombre_Producto_IN.Clear();
            textBox_cantidad_productos_IN.Clear();
            textBox_Precio_Producto_IN.Clear();
        }
        private void button_Atras_Crear_Producto_Click(object sender, EventArgs e)
        {
            groupBox_Info_Producto.Visible = false;
            textBox_Nombre_Producto_IN.Clear();
            textBox_cantidad_productos_IN.Clear();
            textBox_Precio_Producto_IN.Clear();
        }
        private void button_Atras_Producto_Click(object sender, EventArgs e)
        {
            groupBox_Productos.Visible = false;

            textBox_Nombre_Producto_IN.Clear();
            textBox_cantidad_productos_IN.Clear();
            textBox_Precio_Producto_IN.Clear();
        }

        #endregion

        #region VENTAS
        private void button_Ventas_Click(object sender, EventArgs e)
        {
            Form_ventas ventas = new Form_ventas();
            ventas.form_1 = this;
            ventas.ShowDialog();
        }





        #endregion

        private void textBoxDNI_Cliente_IN_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
