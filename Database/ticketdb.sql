-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 16, 2024 at 10:19 PM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ticketdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `akun`
--

CREATE TABLE `akun` (
  `idAkun` int(10) UNSIGNED NOT NULL,
  `nameAkun` varchar(45) DEFAULT NULL,
  `usernameAkun` varchar(15) DEFAULT NULL,
  `passwordAkun` varchar(30) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `akun`
--

INSERT INTO `akun` (`idAkun`, `nameAkun`, `usernameAkun`, `passwordAkun`) VALUES
(3, 'Super Admin', 'admin', 'faiz123'),
(4, 'Unsada', 'mahasiswa', '1234');

-- --------------------------------------------------------

--
-- Table structure for table `ticket`
--

CREATE TABLE `ticket` (
  `idTicket` int(10) UNSIGNED NOT NULL,
  `namaTicket` varchar(255) DEFAULT NULL,
  `hargaTicket` double DEFAULT NULL,
  `fromDest` varchar(45) DEFAULT NULL,
  `toDest` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `ticket`
--

INSERT INTO `ticket` (`idTicket`, `namaTicket`, `hargaTicket`, `fromDest`, `toDest`) VALUES
(1, 'Tiket Bus - (Pahala Kencana, Lorena, Kramat Djati Jakarta)', 300000, 'Palembang', 'Jakarta'),
(2, 'Tiket Bus - (Pahala Kencana, Lorena, Kramat Djati Jakarta) Tujuan Bali', 500000, 'Jakarta', 'Bali'),
(3, 'Tiket Bus - (Sinar Jaya, DAMRI, Pahala Kencana, Harapan Jaya, Haryanto)', 250000, 'Surabaya', 'Jakarta'),
(4, 'Tiket Bus - (DAMRI, Harapan Jaya, Mayora Trans)', 290000, 'Jakarta', 'Malang'),
(5, 'Tiket Bus - (Tiara Mas, MG Travel)', 220000, 'Surabaya', 'Bali'),
(6, 'Tiket Bus - (NPM)', 230000, 'Pekanbaru', 'Medan'),
(7, 'Tiket Bus - (SEMBODO, NPM, Transport Jaya)', 430000, 'Padang', 'Jakarta'),
(8, 'Tiket Bus - (Lorena)', 555000, 'Jambi', 'Jakarta'),
(9, 'Tiket Bus - (Wisata Komodo, Safari Dharma Raya)', 330000, 'Jogja', 'Bali'),
(10, 'Tiket Bus - (Day Trans, Jackal Holidays, Cititrans, Baraya Travel)', 135000, 'Jakarta', 'Bandung'),
(11, 'Tiket Bus - (Lintas Shuttle, Aragon Shuttle, Sinar Shuttle)', 110000, 'Bogor', 'Bandung'),
(12, 'Tiket Bus - (Sinar Jaya, Harapan Jaya, Haryanto)', 230000, 'Jakarta', 'Semarang'),
(13, 'Tiket Bus - (Sabila Shuttle, Day Trans)', 115000, 'Jogja', 'Semarang'),
(14, 'Tiket Bus - (Areon Trans, Lintas Shuttle, Day Trans)', 95000, 'Bekasi', 'Bandung'),
(15, 'Tiket Bus - (Sinar Jaya, Sumber Alam, Handayo)', 205000, 'Joga', 'Jakarta'),
(16, 'Tiket Bus - (Aragon Shuttle, Lintas Shuttle)', 105000, 'Depok', 'Bandung'),
(17, 'Tiket Bus - (Aragon Shuttle)', 110000, 'Semarang', 'Purwokerto'),
(18, 'Tiket Bus - (Kencana Travel, Cititrans)', 112000, 'Semarang', 'Solo');

-- --------------------------------------------------------

--
-- Table structure for table `transaction`
--

CREATE TABLE `transaction` (
  `akun_idAkun` int(10) UNSIGNED NOT NULL,
  `ticket_idTicket` int(10) UNSIGNED NOT NULL,
  `jumlah` int(5) UNSIGNED DEFAULT NULL,
  `total_harga` double DEFAULT NULL,
  `tanggal` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transaction`
--

INSERT INTO `transaction` (`akun_idAkun`, `ticket_idTicket`, `jumlah`, `total_harga`, `tanggal`) VALUES
(3, 4, 6, 660000, '2024-01-17 03:24:37');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `akun`
--
ALTER TABLE `akun`
  ADD PRIMARY KEY (`idAkun`);

--
-- Indexes for table `ticket`
--
ALTER TABLE `ticket`
  ADD PRIMARY KEY (`idTicket`);

--
-- Indexes for table `transaction`
--
ALTER TABLE `transaction`
  ADD PRIMARY KEY (`akun_idAkun`,`ticket_idTicket`),
  ADD KEY `akun_has_ticket_FKIndex1` (`akun_idAkun`),
  ADD KEY `akun_has_ticket_FKIndex2` (`ticket_idTicket`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `akun`
--
ALTER TABLE `akun`
  MODIFY `idAkun` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `ticket`
--
ALTER TABLE `ticket`
  MODIFY `idTicket` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `transaction`
--
ALTER TABLE `transaction`
  ADD CONSTRAINT `transaction_ibfk_1` FOREIGN KEY (`akun_idAkun`) REFERENCES `akun` (`idAkun`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `transaction_ibfk_2` FOREIGN KEY (`ticket_idTicket`) REFERENCES `ticket` (`idTicket`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
