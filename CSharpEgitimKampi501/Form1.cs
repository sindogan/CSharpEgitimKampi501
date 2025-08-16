﻿using CSharpEgitimKampi501.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CSharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Server=localhost\SQLEXPRESS;initial Catalog=EgitimKampi501Db;integrated security=true");
        

      
        private async void btnList_Click(object sender, EventArgs e)
        {
            string query = "Select*From TblProduct";
            var values = await connection.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values;

        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "insert into TblProduct(ProductName,ProductStock,ProductPrice,ProductCategory) values(@productName,@productStock,@productPrice,@productCategory)";
            var parameters = new DynamicParameters();
            parameters.Add("@productName", txtProductName.Text);
            parameters.Add("@productStock",txtProductStock.Text);
            parameters.Add("@productPrice",txtProductPrice.Text);
            parameters.Add("@productCategory",txtProductCategory.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Yeni Kitap Ekleme İşlemi Başarılı");
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "Delete From TblProduct Where ProductId = @productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Silme İşlemi Başarılı");

        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "Update  TblProduct Set  ProductName = @productName,ProductStock=@productStock,ProductPrice=@productPrice,ProductCategory=@productCategory where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", txtProductId.Text);
            parameters.Add("@productName",txtProductName.Text);
            parameters.Add("@productStock",txtProductStock.Text);
            parameters.Add("@productPrice",txtProductPrice.Text);
            parameters.Add("@productCategory",txtProductCategory.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Güncelleme İşlemi Başarılı");


        }

        private  async void Form1_Load(object sender, EventArgs e)
        {
            string query1 = "Select Count(*) From TblProduct";
            var productTotalCount = await connection.QueryFirstOrDefaultAsync<int>(query1);
            lblTotalProductCount.Text = productTotalCount.ToString();


            string query2 = "Select ProductName From TblProduct Where ProductPrice=(Select Max(ProductPrice) From TblProduct)";
            var maxPriceProductName = await connection.QueryFirstOrDefaultAsync<string>(query2);
            lblMaxPriceProductName.Text = maxPriceProductName.ToString();

            string query3 = "Select Count(Distinct(ProductCategory)) From TblProduct";
            var distinctProductCategoryCount = await connection.QueryFirstOrDefaultAsync<int>(query3);
            lblDistinctProductCategoryCount.Text = distinctProductCategoryCount.ToString();

        }
    }
}
