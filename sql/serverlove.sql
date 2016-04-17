-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 21-03-2016 a las 03:17:09
-- Versión del servidor: 10.1.8-MariaDB
-- Versión de PHP: 5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de datos: `serverlove`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `amor`
--

CREATE TABLE IF NOT EXISTS `amor` (
  `idAmor` int(11) NOT NULL AUTO_INCREMENT,
  `idUsuario` int(11) NOT NULL,
  `nombre` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `correo` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `telefono` int(20) NOT NULL,
  `facebook` varchar(30) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`idAmor`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='amor del usuario de la aplicacion movil' AUTO_INCREMENT=2 ;

--
-- Volcado de datos para la tabla `amor`
--

INSERT INTO `amor` (`idAmor`, `idUsuario`, `nombre`, `correo`, `telefono`, `facebook`) VALUES
(1, 1, 'Luis', 'luis@hotmail.com', 317789567, 'LuisM');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imagenamor`
--

CREATE TABLE IF NOT EXISTS `imagenamor` (
  `idImagen` int(11) NOT NULL AUTO_INCREMENT,
  `idAmor` int(11) NOT NULL,
  `imagen` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`idImagen`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci AUTO_INCREMENT=2 ;

--
-- Volcado de datos para la tabla `imagenamor`
--

INSERT INTO `imagenamor` (`idImagen`, `idAmor`, `imagen`) VALUES
(1, 1, 'Img1.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `reconocimiento`
--

CREATE TABLE IF NOT EXISTS `reconocimiento` (
  `idUsuario` int(11) NOT NULL,
  `idAmor` int(11) NOT NULL,
  `amorValido` int(11) NOT NULL,
  `enviarDulce` int(11) NOT NULL,
  `idReconocimiento` int(11) NOT NULL AUTO_INCREMENT,
  `enviarMsjs` int(11) NOT NULL,
  PRIMARY KEY (`idReconocimiento`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='Tabla que registra el log de reconocimiento del amor en el sistema' AUTO_INCREMENT=3 ;

--
-- Volcado de datos para la tabla `reconocimiento`
--

INSERT INTO `reconocimiento` (`idUsuario`, `idAmor`, `amorValido`, `enviarDulce`, `idReconocimiento`, `enviarMsjs`) VALUES
(1, 1, 0, 0, 1, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE IF NOT EXISTS `usuario` (
  `idUsuario` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `correo` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `clave` varchar(20) COLLATE utf8_unicode_ci NOT NULL,
  `telefono` int(11) NOT NULL,
  PRIMARY KEY (`idUsuario`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='Se registran los datos de los usuarios de la appMovil' AUTO_INCREMENT=2 ;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`idUsuario`, `nombre`, `correo`, `clave`, `telefono`) VALUES
(1, 'Luisa', 'lisby218@hotmail.com', 'lisby218!', 317654321);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
