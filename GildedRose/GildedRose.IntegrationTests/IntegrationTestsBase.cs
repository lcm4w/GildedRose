using AutoMapper;
using GildedRose.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Migrations;
using System.Transactions;

namespace GildedRose.IntegrationTests
{
	public class IntegrationTestsBase
	{
		private TransactionScope _transactionScope;

		[TestInitialize]
		public void Initialize()
		{
			InitializeDatabase();
			_transactionScope = new TransactionScope();

			Mapper.Reset();
			Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
		}

		/// <summary>
		/// This is caling the migrations from GildedRose project.
		/// This will run every time but the actual DB creation will happen only
		/// if there is no existing DB.
		/// <remarks>
		/// SEEDS:
		/// Users table - 1
		/// Items table - 4
		/// </remarks>
		/// </summary>
		private void InitializeDatabase()
		{
			var configuration = new Migrations.Configuration();
			var migrator = new DbMigrator(configuration);
			migrator.Update();
		}

		[TestCleanup]
		public void TestCleanup()
		{
			_transactionScope.Dispose();
		}
	}
}