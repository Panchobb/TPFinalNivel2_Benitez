using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Dominio;
using Negocio;
using System.Security.Cryptography;
using Gestor_de_Catalogo;

namespace Gestor
{
    public partial class Servicios : Form
    {
        private List<Articulos1> listaArticulos;
        public Servicios()
        {
            InitializeComponent();

        }

        private void Servicios_Load(object sender, EventArgs e)
        {
            Cargar();
            cbxCampo.Items.Add("Código");
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Marca");
            cbxCampo.Items.Add("Categoría");
            cbxCampo.Items.Add("Precio");

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (Dgv_Articulos.CurrentRow == null || Dgv_Articulos.CurrentRow.DataBoundItem == null)
                return;

            Articulos1 seleccionado = (Articulos1)Dgv_Articulos.CurrentRow.DataBoundItem;
            CargarImagen(seleccionado.ImagenUrl);

            txtCodigo.Text = seleccionado.Codigo;
            txtNombre.Text = seleccionado.Nombre;
            txtDescripcion.Text = seleccionado.Descripcion;
            txtMarca.Text = seleccionado.marca.Descripcion;
            txtCategoria.Text = seleccionado.categorias.Descripcion;
            txtPrecio.Text = seleccionado.Precio.ToString("0.00");



        }
        private void Cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulos = negocio.listar();
                Dgv_Articulos.DataSource = listaArticulos;
                OcultarColumnas();
                if (listaArticulos.Count > 0)
                    CargarImagen(listaArticulos[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void OcultarColumnas()
        {
            Dgv_Articulos.Columns["ImagenUrl"].Visible = false;
            Dgv_Articulos.Columns["Id"].Visible = false;
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbx_Articulos.Load(imagen);
            }
            catch (Exception)
            {
                pbx_Articulos.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar alta = new frmAgregar();
            alta.ShowDialog();
            Cargar();
        }

        private void Dgv_Articulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            if (Dgv_Articulos.CurrentRow == null)
            {
                MessageBox.Show("Por favor, selecciona un artículo.");
                return;
            }

            Articulos1 seleccionado = (Articulos1)Dgv_Articulos.CurrentRow.DataBoundItem;


            frmAgregar modificar = new frmAgregar(seleccionado);

            modificar.ShowDialog();
            Cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulos1 seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Estás seguro de que deseas eliminar este artículo?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulos1)Dgv_Articulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    Cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eliminado Exitosamente");
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {



        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {

        }



        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Mayor a");
                cbxCriterio.Items.Add("Menor a");
                cbxCriterio.Items.Add("Igual a");
            }
            else
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Comienza con");
                cbxCriterio.Items.Add("Termina con");
                cbxCriterio.Items.Add("Contiene");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
        }
        private void validarFiltro()
        {
            if (cbxCampo.SelectedIndex < 0)
                throw new Exception("Por favor, seleccione un campo para filtrar.");
            if (cbxCriterio.SelectedIndex < 0)
                throw new Exception("Por favor, seleccione un criterio para filtrar.");
            if (string.IsNullOrWhiteSpace(txtFiltro.Text))
                throw new Exception("Por favor, ingrese un valor para filtrar.");
            if (cbxCampo.SelectedItem.ToString() == "Precio")
            {
                if (!decimal.TryParse(txtFiltro.Text, out _))
                    throw new Exception("Por favor, ingrese un valor numérico válido para el precio.");
            }
        }
        private void btnBuscar_Click_1(object sender, EventArgs e)
        {

            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                string campo = cbxCampo.SelectedItem?.ToString();
                string criterio = cbxCriterio.SelectedItem?.ToString();
                string filtro = txtFiltro.Text;
                Dgv_Articulos.DataSource = negocio.filtrado(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Por favor, complete todos los campos de búsqueda.");


            }

        }
    }
}
