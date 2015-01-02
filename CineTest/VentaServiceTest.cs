﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Cine;
using Cine.Interfaces;
namespace CineTest
{
    [TestClass]
    public class VentaServiceTest
    {
        private VentaService sut;
        private Mock<IVentaRepository> mockVentaRepository;
        private Mock<ISesionService> mockSesionService;
        private Mock<ISalaService> mockSalaService;

        [TestInitialize]
        public void TestInicializa()
        {
            mockVentaRepository = new Mock<IVentaRepository>();
            mockSesionService = new Mock<ISesionService>();
            mockSalaService = new Mock<ISalaService>();
            sut = new VentaService(mockVentaRepository.Object, mockSesionService.Object, mockSalaService.Object);

        }
        #region mockConfigurations
        //venta repository
        private void SetupRepoCreate()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Create(It.IsAny<Venta>()))
                .Returns(
                    (Venta v) =>
                    {
                        v.Id = 1;
                        return v;
                    }
                );
        }
        private void VerifyRepoCreate(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.Create(It.IsAny<Venta>()), Times.Exactly(times));
        }
        private void SetupRepoRead()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Read(It.IsAny<long>()))
                .Returns((long id) => { return new Venta { SesionId = 1, NumeroEntradas = 20, Id = id, AppliedDiscount = 10, PrecioEntrada = 7.0d, DiferenciaDevolucion = 0, TotalVenta = 126.0d }; });
        }
        private void SetupRepoReadNull()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Read(It.IsAny<long>()))
                .Returns<Venta>(null);
        }
        private void VerifyRepoRead(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.Read(It.IsAny<long>()), Times.Exactly(times));
        }
        private void SetupRepoList20Spots()
        {
            mockVentaRepository.Setup(vRepository => vRepository.List(It.IsAny<long>()))
                .Returns(new Dictionary<long, Venta>(){
                    {1, new Venta(1, 40)},
                    {2, new Venta(1, 40)}
                });
        }
        private void SetupRepoListFull()
        {
            mockVentaRepository.Setup(vRepository => vRepository.List(It.IsAny<long>()))
                .Returns(new Dictionary<long, Venta>(){
                    {1,new Venta(1,50)},
                    {2,new Venta(1,50)}
                });
        }
        private void SetupRepoListTotales()
        {
            mockVentaRepository.Setup(vRepository => vRepository.List(It.IsAny<long>()))
                .Returns(
                    (long caso) =>
                    {
                        if (caso == -1)
                        {
                            return new Dictionary<long, Venta>(){
                                {1, new Venta{Id = 1, SesionId = 1, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                                {2, new Venta{Id = 1, SesionId = 2, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                                {3, new Venta{Id = 1, SesionId = 3, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                                {4, new Venta{Id = 1, SesionId = 4, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                                {5, new Venta{Id = 1, SesionId = 5, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                                {6, new Venta{Id = 1, SesionId = 6, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                                {7, new Venta{Id = 1, SesionId = 7, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                                {8, new Venta{Id = 1, SesionId = 8, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                                {9, new Venta{Id = 1, SesionId = 9, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                            };
                        }
                        else
                        {
                            return new Dictionary<long, Venta>()
                            {
                                {caso, new Venta{Id = 1, SesionId = caso, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0} },
                            };
                        }

                    });
        }
        private void VerifyRepoList(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.List(It.IsAny<long>()), Times.Exactly(times));
        }
        private void SetupRepoUpdate()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Update(It.IsAny<Venta>()))
                .Returns((Venta v) => (v));
        }
        private void VerifyRepoUpdate(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.Update(It.IsAny<Venta>()), Times.Exactly(times));
        }
        private void SetupRepoDelete()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Delete(It.IsAny<long>()))
                .Returns((long id) => { return new Venta { SesionId = 1, Id = id, NumeroEntradas = 20 }; });
        }
        private void VerifyRepoDelete(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.Delete(It.IsAny<long>()), Times.Exactly(times));
        }
        //sesion service 
        private void SetupSesionRead(bool abierta = false)
        {
            mockSesionService.Setup(sesService => sesService.Read(It.IsIn<long>(Constantes.Sesiones)))
               .Returns(
                   (long id) =>
                   {
                       Sesion sesion = new Sesion(id, 1, "17:00");
                       sesion.EstaAbierta = abierta;
                       return sesion;
                   }
               );
        }
        private void VerifySesionRead(int times)
        {
            mockSesionService.Verify(sesService => sesService.Read(It.IsIn<long>(Constantes.Sesiones)), Times.Exactly(times));
        }
        private void SetupSesionList()
        {
            mockSesionService.Setup(sesService => sesService.List(It.IsIn<long>(Constantes.Salas)))
                .Returns(
                    (long id) =>
                    {
                        long first = 1 + 3 * (id-1);
                        return new Dictionary<long, Sesion>() {
                            {first, new Sesion(first++,id,"unahora")},
                            {first, new Sesion(first++,id,"unahora")},
                            {first, new Sesion(first,id,"unahora")}
                        };
                    }
                );
        }
        private void VerifySesionList(int times)
        {
            mockSesionService.Verify(sesService => sesService.List(It.IsIn<long>(Constantes.Sesiones)), Times.Exactly(times));
        }
        //sala service 
        private void SetupSalaRead()
        {
            mockSalaService.Setup(sService => sService.Read(1))
                .Returns(new Sala(1, 100));
        }
        private void VerifySalaRead(int times)
        {
            mockSalaService.Verify(sService => sService.Read(1), Times.Exactly(times));
        }
        #endregion

        #region helperTests
        [TestMethod]
        public void TestHaySuficientesButacas()
        {
            SetupSalaRead();
            SetupRepoList20Spots();
            bool isEnough = sut.HaySuficientesButacas(new Sesion(1, 1, "17:00"), new Venta(1, 20));
            Assert.IsTrue(isEnough);
            VerifySalaRead(1);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestHaySuficientesButacasNoHay()
        {
            SetupSalaRead();
            SetupRepoList20Spots();
            bool isEnough = sut.HaySuficientesButacas(new Sesion(1, 1, "17:00"), new Venta(1, 21));
            Assert.IsFalse(isEnough);
            VerifySalaRead(1);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestHaySuficientesButacasDevolucion()
        {
            SetupSalaRead();
            SetupRepoList20Spots();
            bool isEnough = sut.HaySuficientesButacas(new Sesion(1, 1, "17:00"), new Venta(1, 30), new Venta(1, 10));
            Assert.IsTrue(isEnough);
            VerifySalaRead(1);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestHaySuficientesButacasNoHayDevolucion()
        {
            SetupSalaRead();
            SetupRepoList20Spots();
            bool isEnough = sut.HaySuficientesButacas(new Sesion(1, 1, "17:00"), new Venta(1, 31), new Venta(1, 10));
            Assert.IsFalse(isEnough);
            VerifySalaRead(1);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalculaPrecioYDescuento()
        {
            Venta venta = new Venta(1, 4);
            venta = sut.CalculaPrecioYDescuento(venta);
            Assert.AreEqual(28.0d, venta.TotalVenta);
            Assert.AreEqual(0, venta.AppliedDiscount);
        }

        [TestMethod]
        public void TestCalculaPrecioYDescuentoConDescuento()
        {
            Venta venta = new Venta(1, 6);
            venta = sut.CalculaPrecioYDescuento(venta);
            Assert.AreEqual(37.8d, venta.TotalVenta, 0.01d);
            Assert.AreEqual(10, venta.AppliedDiscount);
        }
        [TestMethod]
        public void TestCalculaPrecioYDescuentoDiferencia()
        {
            SetupRepoRead();
            Venta venta = new Venta { Id = 1, SesionId = 1, NumeroEntradas = 4 };
            Venta antiguaVenta = sut.Read(1);
            venta = sut.CalculaPrecioYDescuento(venta, antiguaVenta);
            Assert.AreEqual(28.0d, venta.TotalVenta, 0.01d);
            Assert.AreEqual(0, venta.AppliedDiscount);
            Assert.AreEqual(-98d, venta.DiferenciaDevolucion, 0.001d);
        }
        [TestMethod]
        public void TestCalculaPrecioYDescuentoDiferenciaConDescuento()
        {
            SetupRepoRead();
            Venta venta = new Venta { Id = 1, SesionId = 1, NumeroEntradas = 6 };
            Venta antiguaVenta = sut.Read(1);
            venta = sut.CalculaPrecioYDescuento(venta, antiguaVenta);
            Assert.AreEqual(37.8d, venta.TotalVenta, 0.01d);
            Assert.AreEqual(10, venta.AppliedDiscount);
            Assert.AreEqual(-88.2d, venta.DiferenciaDevolucion, 0.001d);
        }
        #endregion

        #region CreateTests
        [TestMethod]
        public void TestCreate()
        {
            SetupSalaRead();
            SetupSesionRead(true);
            SetupRepoCreate();
            SetupRepoList20Spots();

            Venta res = sut.Create(new Venta(1, 2));
            Assert.AreEqual(14.0, res.TotalVenta);
            Assert.AreEqual(0, res.AppliedDiscount);
            Assert.AreEqual(Constantes.TicketPrice, res.PrecioEntrada);

            VerifySalaRead(1);
            VerifySesionRead(1);
            VerifyRepoCreate(1);
            VerifyRepoList(1);
        }

        [TestMethod]
        public void TestCreateConDescuento()
        {
            SetupSalaRead();
            SetupSesionRead(true);
            SetupRepoCreate();
            SetupRepoList20Spots();

            Venta res = sut.Create(new Venta(1, 8));
            Assert.AreEqual(50.4d, res.TotalVenta, 0.01d);
            Assert.AreEqual(10, res.AppliedDiscount);
            Assert.AreEqual(Constantes.TicketPrice, res.PrecioEntrada);

            VerifySalaRead(1);
            VerifySesionRead(1);
            VerifyRepoCreate(1);
            VerifyRepoList(1);
        }

        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestCreateSesionCerrada()
        {
            SetupSesionRead(false);
            sut.Create(new Venta(1, 2));
        }

        [TestMethod]
        [ExpectedException(typeof(VentaExceptionNoAforo))]
        public void TestCreateNoHayButacas()
        {
            SetupSalaRead();
            SetupSesionRead(true);
            SetupRepoListFull();
            sut.Create(new Venta(1, 2));
        }
        #endregion

        #region ReadTests
        [TestMethod]
        public void TestRead()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Read(1))
                .Returns((long id) => { Venta venta = new Venta(1, 20); venta.Id = id; return venta; });
            sut.Read(1);
            mockVentaRepository.Verify(vRepository => vRepository.Read(1), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestReadNoExisteVenta()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Read(1)).Returns<Venta>(null);
            sut.Read(1);
        }
        #endregion

        #region ListTests
        [TestMethod]
        public void TestList()
        {
            SetupRepoList20Spots();
            sut.List();
            VerifyRepoList(1);
        }
        #endregion

        #region UpdateTests
        [TestMethod]
        public void TestUpdate()
        {
            SetupSalaRead();
            SetupSesionRead(true);
            SetupRepoList20Spots();
            SetupRepoRead();
            SetupRepoUpdate();

            Venta paraActualizar = new Venta(1, 4);
            paraActualizar.Id = 1;
            Venta res = sut.Update(paraActualizar);
            Assert.AreEqual(28.0d, res.TotalVenta, 0.01d);
            Assert.AreEqual(0, res.AppliedDiscount);
            Assert.AreEqual(-98d, res.DiferenciaDevolucion, 0.001d);

            VerifySalaRead(1);
            VerifySesionRead(1);
            VerifyRepoList(1);
            VerifyRepoRead(1);
            VerifyRepoUpdate(1);
        }
        [TestMethod]
        public void TestUpdateConDescuento()
        {
            SetupSalaRead();
            SetupSesionRead(true);
            SetupRepoList20Spots();
            SetupRepoRead();
            SetupRepoUpdate();

            Venta paraActualizar = new Venta(1, 19);
            paraActualizar.Id = 1;
            Venta res = sut.Update(paraActualizar);
            Assert.AreEqual(119.7d, res.TotalVenta, 0.01d);
            Assert.AreEqual(10, res.AppliedDiscount);
            Assert.AreEqual(-6.3d, res.DiferenciaDevolucion, 0.001d);

            VerifySalaRead(1);
            VerifySesionRead(1);
            VerifyRepoList(1);
            VerifyRepoRead(1);
            VerifyRepoUpdate(1);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestUpdateNoExisteVenta()
        {
            SetupRepoReadNull();

            Venta paraActualizar = new Venta(1, 20);
            paraActualizar.Id = 1;
            sut.Update(paraActualizar);
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestUpdateSesionCerrada()
        {
            SetupRepoRead();
            SetupSesionRead(false);

            Venta paraActualizar = new Venta(1, 20);
            paraActualizar.Id = 1;
            sut.Update(paraActualizar);

            VerifyRepoRead(1);
            VerifySesionRead(1);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaExceptionNoAforo))]
        public void TestUpdateNoHayButacas()
        {
            SetupSalaRead();
            SetupSesionRead(true);
            SetupRepoList20Spots();
            SetupRepoRead();
            Venta paraActualizar = new Venta(1, 41);
            paraActualizar.Id = 1;
            sut.Update(paraActualizar);
        }
        #endregion

        #region DeleteTest
        [TestMethod]
        public void TestDelete()
        {
            SetupSesionRead(true);
            SetupRepoRead();
            SetupRepoDelete();

            sut.Delete(1);

            VerifySesionRead(1);
            VerifyRepoRead(1);
            VerifyRepoDelete(1);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestDeleteNoExisteVenta()
        {
            SetupRepoReadNull();
            sut.Delete(1);
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestDeleteSesionCerrada()
        {
            SetupRepoRead();
            SetupSesionRead(false);
            sut.Delete(1);
        }
        #endregion
        #region TotalizeTests
        [TestMethod]
        public void TestCalCularTotales()
        {
            SetupRepoListTotales();
            double res = sut.CalcularTotales();
            Assert.AreEqual(1134.0d, res, 0.001d);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalcularTotalesBySesion()
        {
            SetupRepoListTotales();
            double res = sut.CalcularTotales(1);
            Assert.AreEqual(126.0d, res, 0.001d);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalcularTotalesBySala()
        {
            SetupRepoListTotales();
            SetupSesionList();
            double res = sut.CalcularTotales(-1, 1);
            Assert.AreEqual(378.0d, res, 0.001d);
            VerifyRepoList(3);
            VerifySesionList(1);
        }
        [TestMethod]
        public void TestCalCularEntradas()
        {
            SetupRepoListTotales();
            double res = sut.CalcularEntradas();
            Assert.AreEqual(180, res);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalcularEntradasBySesion()
        {
            SetupRepoListTotales();
            double res = sut.CalcularEntradas(1);
            Assert.AreEqual(20, res);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalcularEntradasBySala()
        {
            SetupRepoListTotales();
            SetupSesionList();
            double res = sut.CalcularEntradas(-1, 1);
            Assert.AreEqual(60, res);
            VerifyRepoList(3);
            VerifySesionList(1);
        }
        #endregion
    }
}