using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestor_de_Catalogo
{
    public partial class frmAgregar : Form
    {
        private Articulos1 articulo = null;
        private OpenFileDialog archivo = null;
        public frmAgregar()
        {
            InitializeComponent();
        }
        public frmAgregar(Articulos1 articulos)
        {

            InitializeComponent();
            this.articulo = articulos;
            Text = "Modificar Articulo";
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!validarDatos())
                return;

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
              
                if (articulo == null)
                    articulo = new Articulos1();

                
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Precio = int.Parse(txtPrecio.Text);
                articulo.marca = (Marca)cbxMarca.SelectedItem;
                articulo.categorias = (Categorias)cbxCategoria.SelectedItem;

               
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    string rutaCarpeta = ConfigurationManager.AppSettings["images-folder"];
                    string nombreArchivo = archivo.SafeFileName;
                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                    if (!Directory.Exists(rutaCarpeta))
                        Directory.CreateDirectory(rutaCarpeta);

                    File.Copy(archivo.FileName, rutaCompleta, true);
                    articulo.ImagenUrl = rutaCompleta;
                }
                else
                {
                    articulo.ImagenUrl = txtUrlImagen.Text;
                }

                
                if (articulo.Id != 0)
                {
                    negocio.modificado(articulo);
                    MessageBox.Show("Artículo modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Artículo agregado exitosamente");
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private bool validarDatos()
        {
            string mensaje = ""; 

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El campo 'Código' no puede estar vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
                

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El campo 'Nombre' no puede estar vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
             
            if (string.IsNullOrWhiteSpace(txtPrecio.Text)) {
                MessageBox.Show("El campo 'Precio' no puede estar vacío.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
                

            if (!string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show("Para agregar un artículo, debe completar los siguientes campos:\n\n" + mensaje, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; 
            }

            return true; 
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {


            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {

                cbxCategoria.DataSource = categoriaNegocio.Listar();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";

                cbxMarca.DataSource = marcaNegocio.Listar();
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";


                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    CargarImagen(articulo.ImagenUrl);


                    cbxMarca.SelectedValue = articulo.marca.Id;
                    cbxCategoria.SelectedValue = articulo.categorias.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void cbxMarca_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtUrlImagen.Text);
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbcxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbcxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);
            }
        }
    }
}


