using CandidateTest.WinForms.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CandidateTest.WinForms
{
    public partial class ProductsForm : Form
    {
        private readonly ProductApiService _apiService;
        private List<ProductDto> _products = new List<ProductDto>();
        private int _page = 1;
        private int _pageSize = 10;   
        private int _total = 0;
        public ProductsForm()
        {
            InitializeComponent();

            _apiService = new ProductApiService("https://localhost:5001/");

            // Registracija event handler-a nakon InitializeComponent
            btnRefresh.Click += BtnRefresh_Click;
            btnCreate.Click += BtnCreate_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnPrev.Click += BtnPrev_Click;
            btnNext.Click += BtnNext_Click;

            this.Load += ProductsForm_Load;
        }
        private async void BtnPrev_Click(object sender, EventArgs e)
        {
            if (_page > 1)
            {
                _page--;
                await RefreshProducts();
            }
        }

        private async void BtnNext_Click(object sender, EventArgs e)
        {
            _page++;
            await RefreshProducts();
        }

        private async void ProductsForm_Load(object sender, EventArgs e)
        {
            await RefreshProducts();
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshProducts();
        }

        private async Task RefreshProducts()
        {
            var (items, total) = await _apiService.GetPagedAsync(
                page: _page,
                pageSize: _pageSize,
                name: null,
                minPrice: null,
                maxPrice: null,
                categoryIds: null
            );

            _products = items;
            _total = total;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _products;
            dataGridView1.ClearSelection();

            var maxPage = Math.Max(1, (int)Math.Ceiling((double)_total / _pageSize));
            lblPageInfo.Text = $"Page: {_page} / {maxPage}   Total: {_total}";

            btnPrev.Enabled = _page > 1;
            btnNext.Enabled = _page < maxPage;
        }
        private void ClearInputs()
        {
            txtName.Clear(); txtPrice.Clear(); txtDescription.Clear(); txtQuantity.Clear();
        }

        private async void BtnCreate_Click(object sender, EventArgs e)
        {
            var create = new ProductCreateDto
            {
                ProductName = txtName.Text,
                Price = decimal.TryParse(txtPrice.Text, out var p) ? p : 0,
                Description = txtDescription.Text,
                StockQuantity = int.TryParse(txtQuantity.Text, out var q) ? q : 0
            };
            await _apiService.CreateAsync(create);
            _page = 1;
            await RefreshProducts();
        }

        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            var selected = (ProductDto)dataGridView1.CurrentRow.DataBoundItem;
            var update = new ProductUpdateDto
            {
                ProductId = selected.ProductId,
                ProductName = txtName.Text,
                Price = decimal.TryParse(txtPrice.Text, out var p2) ? p2 : 0,
                Description = txtDescription.Text,
                StockQuantity = int.TryParse(txtQuantity.Text, out var q2) ? q2 : 0
            };
            await _apiService.UpdateAsync(selected.ProductId, update);
            _page = 1;
            await RefreshProducts();
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            var selected = (ProductDto)dataGridView1.CurrentRow.DataBoundItem;

            await _apiService.DeleteAsync(selected.ProductId);
            _page = 1;
            await RefreshProducts();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            var selected = (ProductDto)dataGridView1.CurrentRow.DataBoundItem;

            txtName.Text = selected.ProductName;
            txtPrice.Text = selected.Price.ToString();
            txtDescription.Text = selected.Description;
            txtQuantity.Text = selected.StockQuantity.ToString();
        }
    }
}
