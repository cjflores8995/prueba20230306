using Aplicacion.Helper;

using AutoFixture;

using AutoMapper;

using Dominio.Modelos;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Logica.Cuentas
{
    [TestFixture]
    public class ConsultaNUnitTest
    {
        private Cuentas.Consulta.Manejador handlerAddCuentas;

        [SetUp]
        public void Setup()
        {
            // permite crear la data de prueba
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            //data de prueba
            var cuentaRecords = fixture.CreateMany<Cuenta>().ToList();



            //agrega un registro con id vacio
            cuentaRecords.Add(fixture.Build<Cuenta>().With(cl => cl.CuentaId, Guid.Empty).Create());

            //fake options para la base de datos
            var options = new DbContextOptionsBuilder<BcoDbContext>()
                .UseInMemoryDatabase(databaseName: $"BcoDbContext-{Guid.NewGuid()}")
                .Options;

            //creo el fake dbContext
            var bancoDbContext = new BcoDbContext(options);

            //agrego la data falsa
            bancoDbContext.Cuenta.AddRange(cuentaRecords);
            bancoDbContext.SaveChanges();

            // Emulacion del Mapper
            var mapConfing = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });
            var mapper = mapConfing.CreateMapper();

            //instancio el objeto de consulta
            handlerAddCuentas = new Consulta.Manejador(bancoDbContext, mapper);
        }

        [Test]
        public async Task Manejador_ConsultaClientes_Returns_True()
        {
            Consulta.ListaCuentas request = new();
            var resultados = await handlerAddCuentas.Handle(request, new System.Threading.CancellationToken());

            Assert.IsNotNull(resultados);
        }
    }
}
