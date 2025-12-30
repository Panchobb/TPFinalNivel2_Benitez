using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Gestor_de_Catalogo
{
    public partial class frmAgregar : Form
    {
        private Articulos1 articulo = null;
        public frmAgregar()
        {
            InitializeComponent();
        }
        public frmAgregar( Articulos1 articulos)
        {

            InitializeComponent();
            this.articulo = articulos;
            Text= "Modificar Articulo";
        }
        private void btnAceptar_Click(object sender, EventArgs e)

        {

            

            ArticuloNegocio negocio = new ArticuloNegocio();





            try

            {
                if (articulo == null)
                    articulo = new Articulos1();
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.marca = (Marca)cbxMarca.SelectedItem;
                articulo.categorias = (Categorias)cbxCategoria.SelectedItem;
                articulo.ImagenUrl = txtUrlImagen.Text;

                if (articulo.Id != null)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Articulo modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);

                    MessageBox.Show("Articulo agregado exitosamente");
                }

                    
                
                Close();
                


            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.ToString());

            }

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

    }
}


