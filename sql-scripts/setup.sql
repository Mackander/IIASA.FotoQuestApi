USE `fotoquest_db`;

CREATE TABLE `imagedata` (
  `Id` char(36) NOT NULL,
  `OriginalName` varchar(200) DEFAULT NULL,
  `DateOfUpload` datetime DEFAULT NULL,
  `HorizontalResolution` float DEFAULT NULL,
  `VerticalResolution` float DEFAULT NULL,
  `Height` int DEFAULT NULL,
  `Width` int DEFAULT NULL,
  `FilePath` varchar(1000) DEFAULT NULL,
  `FileName` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`)
);

  
  
USE `fotoquest_db`;
DROP procedure IF EXISTS `uspStoreImageData`;

USE `fotoquest_db`;
DROP procedure IF EXISTS `fotoquest_db`.`uspStoreImageData`;
;

DELIMITER $$
USE `fotoquest_db`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `uspStoreImageData`(
IN Id CHAR(36), 
IN OriginalName VARCHAR(200),
IN DateOfUpload DATETIME,
IN HorizontalResolution FLOAT,
IN VerticalResolution FLOAT,
IN Height INT,
IN Width INT,
IN FilePath VARCHAR(1000),
IN FileName VARCHAR(200)
)
BEGIN
	Insert into imagedata ( Id, OriginalName, DateOfUpload, HorizontalResolution, VerticalResolution, Height, Width, FilePath, FileName )
    value ( Id, OriginalName, DateOfUpload, HorizontalResolution, VerticalResolution, Height, Width, FilePath, FileName);
END$$

DELIMITER ;
;




USE `fotoquest_db`;
DROP procedure IF EXISTS `uspFetchImageData`;

USE `fotoquest_db`;
DROP procedure IF EXISTS `fotoquest_db`.`uspFetchImageData`;
;

DELIMITER $$
USE `fotoquest_db`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `uspFetchImageData`(
IN arg_Id CHAR(36)
)
BEGIN
	Select OriginalName, DateOfUpload, HorizontalResolution, VerticalResolution, Height, Width, FilePath, FileName from imagedata Where Id = arg_Id  limit 1;
END$$

DELIMITER ;
;



