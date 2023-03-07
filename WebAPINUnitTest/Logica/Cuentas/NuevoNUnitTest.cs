using Aplicacion.Helper;

using AutoFixture;

using AutoMapper;

using Dominio.Modelos;

using MediatR;

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
    public class NuevoNUnitTest
    {
        private Cuentas.Nuevo.Manejador handlerCreateCuenta;
        private Guid clienteId;

        [SetUp]
        public void Setup()
        {
            // permite crear la data de prueba
            clienteId = Guid.NewGuid();
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            //data de prueba
            var cuentaRecords = fixture.CreateMany<Cuenta>().ToList();

            var cliente = fixture.Build<Cliente>().With(cl => cl.ClienteId, clienteId).Create();
            var cuenta = fixture.Build<Cuenta>().With(cl => cl.CuentaId, Guid.NewGuid()).Create();
            cuenta.Cliente = cliente;
            cuenta.ClienteId = cliente.ClienteId;


            //agrega un registro con id vacio
            cuentaRecords.Add(cuenta);

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
            handlerCreateCuenta = new Nuevo.Manejador(bancoDbContext);
        }

        [Test]
        public async Task Manejador_CreateCuenta_ReturnsNumber()
        {

            Nuevo.Ejecuta request = new();
            request.ClienteId = clienteId;
            request.NumeroCuenta = "451256";
            request.TipoCuenta = "Ahorros";
            request.SaldoInicial = 100;
            request.Estado = true;


            var resultado = await handlerCreateCuenta.Handle(request, new System.Threading.CancellationToken());

            Assert.That(resultado, Is.EqualTo(Unit.Value));
        }
    }
}
