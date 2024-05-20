CREATE TABLE [dbo].[tblDangKyXoSo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[TuGioDangKy] [datetime] NULL,
	[DenGioDangKy] [datetime] NULL,
	[TrungThuong] [int] NULL,
	[GioThucTe] [datetime] NULL,
	[SoDangKy] [int] NULL,
 CONSTRAINT [PK_tblDangKyXoSo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblEmploy]    Script Date: 5/19/2024 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEmploy](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[HoVaTen] [nvarchar](150) NULL,
	[NgaySinh] [datetime] NULL,
	[NgayVaoApp] [datetime] NULL,
	[SDT] [varchar](15) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Employ] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblKetQua]    Script Date: 5/19/2024 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblKetQua](
	[KetQuaID] [int] IDENTITY(1,1) NOT NULL,
	[KetQuaXoSo] [int] NULL,
	[ThoiGian] [datetime] NULL,
 CONSTRAINT [PK_tblKetQua] PRIMARY KEY CLUSTERED 
(
	[KetQuaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblLog]    Script Date: 5/19/2024 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLog](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_tblLog] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateSlot]    Script Date: 5/19/2024 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Author: Hải
	CreateDate: 18/05/2024
	Desc: Tự động tạo số theo mỗi tiếng
	EXEC sp_CreateSlot
*/
CREATE PROC [dbo].[sp_CreateSlot]
AS
BEGIN
	DECLARE @StartTime DATETIME , @Count INT = 0;
	SELECT TOP(1) @StartTime = ThoiGian FROM tblKetQua ORDER BY KetQuaID DESC

	-- Lấy thời điểm hiện tại
	DECLARE @CurrentTime DATETIME;
	SET @CurrentTime = GETDATE();

	-- Tính khoảng cách thời gian giữa @StartTime và @CurrentTime theo giờ
	DECLARE @HoursDifference INT;
	SET @HoursDifference = DATEDIFF(SECOND, @StartTime, @CurrentTime) / 3600.0;

	WHILE (@Count<@HoursDifference)
		BEGIN
			SET @StartTime = DATEADD(HOUR, 1, @StartTime);
			INSERT INTO tblKetQua VALUES(( FLOOR(RAND() * 10)),@StartTime);
			SET @Count=@Count+1
		END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetListEmploy]    Script Date: 5/19/2024 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Author: Hải
	CreateDate: 16/05/2024
	Desc: Kiểm tra lấy danh sách người dùng đăng ký
*/
CREATE PROC [dbo].[sp_GetListEmploy]
@Activity VARCHAR(100) = NULL
,@Message NVARCHAR(1000) = NULL OUT
,@MessageCode INT = NULL OUT
,@HoTen NVARCHAR(100) = NULL
,@SDT VARCHAR(20) = NULL
,@NgaySinh VARCHAR(20) = NULL
AS
BEGIN
DECLARE @StartDate DATETIME = CONVERT(DATETIME,FORMAT(GETDATE(), 'dd/MM/yyyy HH') + ':00',103)
DECLARE @EndDate DATETIME = DATEADD(HOUR, 1, @StartDate);
DECLARE @dUserID INT
SELECT TOP(1) @dUserID = UserID FROM tblEmploy(NOLOCK) WHERE SDT = @SDT

	IF(@Activity = 'CheckNumber')
	BEGIN
		SELECT TOP(1) HoVaTen FROM tblEmploy(NOLOCK) WHERE SDT = @SDT
		IF Exists (SELECT TOP(1) HoVaTen FROM tblEmploy(NOLOCK) WHERE SDT = @SDT)
		BEGIN
			INSERT INTO TBLlog VALUES(@dUserID,GETDATE())
		END
	END

	ELSE IF(@Activity = 'SaveData')
	BEGIN
		-- Kiểm tra thông tin
		IF (LEN(@SDT) > 10 OR LEN(@SDT) < 10)
		BEGIN
			SET @Message = N'Số điện thoại phải là 10 số'
			SET @MessageCode = 0;
			RETURN
		END
		-- kiểm tra trùng người đăng ký
		IF Exists(SELECT TOP(1) HoVaTen FROM tblEmploy(NOLOCK) WHERE HoVaTen = @HoTen OR SDT = @SDT)
			BEGIN
				SET @Message = N'Bạn đã đăng ký trên hệ thống rồi'
				SET @MessageCode = 0;
				RETURN
			END

		INSERT INTO [dbo].[tblEmploy]([HoVaTen],[NgaySinh],[NgayVaoApp],[SDT],[Status])
		VALUES(@HoTen,CONVERT(DATETIME,@NgaySinh,103),GETDATE(),@SDT,1)

		SET @Message = N'Đăng ký thành công'
		SET @MessageCode = 1;
	END
	
	-- Đăng nhập thành công
	ELSE IF(@Activity = 'GetUserInfo')
	BEGIN
		SELECT TOP(1) HoVaTen,CONVERT(VARCHAR(30),NgaySinh,103) AS NgaySinh
		,SDT, CAST(YEAR(GETDATE()) AS INT) - CAST(YEAR(NgaySinh) AS INT) AS Tuoi
		,B.SoDangKy,FORMAT(B.DenGioDangKy,'dd/MM/yyyy HH:mm') as DenGioDangKy
		,FORMAT(@StartDate,'dd/MM/yyyy HH:mm') AS StartDate
		,FORMAT(B.GioThucTe,'dd/MM/yyyy HH:mm') AS GioThucTe
		FROM (SELECT * FROM tblEmploy(NOLOCK)WHERE SDT = @SDT AND Status = 1) AS A
		LEFT JOIN (SELECT * FROM tblDangKyXoSo(NOLOCK) 
		WHERE GETDATE() BETWEEN TuGioDangKy AND DenGioDangKy) AS B ON A.UserID = B.UserID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetListRegisterInEmp]    Script Date: 5/19/2024 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Author: HaiNT181
	CreateDate: 16/05/2024
	Desc: Danh sách thông tin người dùng và xổ số đăng ký
*/
CREATE PROC [dbo].[sp_GetListRegisterInEmp]
@Activity VARCHAR(100) = NULL
,@Message NVARCHAR(1000) = NULL OUT
,@MessageCode INT = NULL OUT
,@SoDangKy	VARCHAR(10) = NULL
,@NgayHienTai VARCHAR(30) = NULL
,@UserID INT = NULL
,@SDT	VARCHAR(20) = NULL
AS
BEGIN
DECLARE @StartDate DATETIME = CONVERT(DATETIME,FORMAT(GETDATE(), 'dd/MM/yyyy HH') + ':00',103)
DECLARE @EndDate DATETIME = DATEADD(HOUR, 1, @StartDate);
DECLARE @dUserID INT

SELECT TOP(1) @dUserID = UserID FROM tblEmploy(NOLOCK) WHERE SDT = @SDT

	IF(@Activity = 'GetList')
	BEGIN
		SELECT B.HoVaTen, B.SDT,A.SoDangKy
		,CASE WHEN C.KetQuaXoSo IS NULL THEN N'chưa tới giờ xổ số' ELSE CAST(C.KetQuaXoSo AS VARCHAR) END KetQuaXoSo
		,FORMAT(A.GioThucTe,'dd/MM/yyyy HH:mm') AS GioThucTe
		,FORMAT(A.DenGioDangKy,'dd/MM/yyyy HH:mm') AS GioXoSo
		,CASE WHEN A.TrungThuong = 1 THEN N'Trúng thưởng'
		WHEN A.TrungThuong = -1 THEN N'Không trúng'
		ELSE N'Chưa dò' END TrungThuong
		FROM (SELECT * FROM  tblEmploy(NOLOCK)) AS B
		INNER JOIN (SELECT * FROM tblDangKyXoSo(NOLOCK)) AS A ON A.UserID = B.UserID
		LEFT JOIN (SELECT * FROM tblKetQua(NOLOCK)) AS C ON C.ThoiGian = A.DenGioDangKy
		WHERE 1=1
		AND (ISNULL(@SDT,'') = '' OR SDT = @SDT)
		ORDER BY GioXoSo DESC
	END

	ELSE IF(@Activity = 'RanDomNumber')
	BEGIN
		IF Exists(SELECT TOP(1) 1 FROM tblKetQua(NOLOCK) WHERE ThoiGian BETWEEN @StartDate AND @EndDate)
		BEGIN
			SELECT TOP(1) * FROM tblKetQua(NOLOCK) WHERE ThoiGian BETWEEN @StartDate AND @EndDate
			RETURN
		END
		ELSE
		BEGIN
			EXEC sp_CreateSlot -- Load random số theo mỗi giờ
			SELECT TOP(1) * FROM tblKetQua(NOLOCK) WHERE ThoiGian BETWEEN @StartDate AND @EndDate
			RETURN
		END
	END

	ELSE IF(@Activity = 'Register')
	BEGIN
		-- kiểm tra user đã đăng ký slot hiện tại chưa
		IF Exists(SELECT TOP(1) 1 FROM tblDangKyXoSo(NOLOCK) WHERE UserID = @dUserID AND
			GETDATE() BETWEEN TuGioDangKy AND DenGioDangKy )
			BEGIN
				SET @MessageCode = 0
				SET @Message = N'Bạn đã đăng ký slot trong khoản thời gian này rồi, vui lòng đợi quay số và chờ lượt tiếp theo'
				RETURN
			END
		IF (@SoDangKy > 9)
			BEGIN
				SET @MessageCode = 0
				SET @Message = N'Vui lòng nhập số may mắn từ 0 đến 9'
				RETURN
			END
		INSERT INTO tblDangKyXoSo VALUES(@dUserID,@StartDate,@EndDate,0,GETDATE(),@SoDangKy)
		SET @MessageCode = 1
		SET @Message = N'Bạn đã đăng ký thhành công'
		RETURN
	END

	ELSE IF(@Activity = 'CheckRegister')
	BEGIN
		-- Cập nhật số trúng
		UPDATE A
		SET A.TrungThuong =  1
		FROM (SELECT * FROM tblDangKyXoSo(NOLOCK) WHERE TrungThuong = 0) A
		INNER JOIN (SELECT * FROM tblKetQua) AS B ON A.DenGioDangKy = B.ThoiGian 
		AND B.KetQuaXoSo = A.SoDangKy
		-- cập nhật số ko trúng
			UPDATE A
		SET A.TrungThuong =  -1
		FROM (SELECT * FROM tblDangKyXoSo(NOLOCK) WHERE TrungThuong = 0) A
		INNER JOIN (SELECT * FROM tblKetQua) AS B ON A.DenGioDangKy = B.ThoiGian 
		AND B.KetQuaXoSo <> A.SoDangKy
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ListOfStatistics]    Script Date: 5/19/2024 3:13:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Author: HaiNT1
	CreateDate: 19/05/2024
	Desc: Danh sách thống kê
	EXEC sp_ListOfStatistics @TuNgay='01/05/2024', @DenNgay = '30/05/2024'
	
*/
CREATE PROC [dbo].[sp_ListOfStatistics]
@TuNgay AS VARCHAR(30) = NULL
,@DenNgay AS VARCHAR(30) = NULL
AS
BEGIN
DECLARE @StartDate DATETIME = CONVERT(DATETIME,@TuNgay,103)
DECLARE @EndDate DATETIME = CONVERT(DATETIME,@DenNgay,103)

	SELECT Distinct A.CountUser AS SoLuongVaoApp
	,E1.NguoiDungApp,T.TongLuotQuaySo,T2.TongLuotTrung,T3.TongKhongLuotTrung
	,T4.TongLuotChuaDo

	FROM (SELECT COUNT(UserID) AS CountUser,UserID FROM tblLog(NOLOCK) 
	WHERE CreateDate BETWEEN @StartDate AND @EndDate
	GROUP BY UserID) AS A
	-- Người dùng app
	LEFT JOIN (SELECT COUNT(UserID) AS NguoiDungApp FROM tblEmploy(NOLOCK)
	WHERE NgayVaoApp BETWEEN @StartDate AND @EndDate) AS E1 ON 1=1
	-- Tổng lượt quay số
	OUTER APPLY(
		SELECT COUNT(UserID) AS TongLuotQuaySo FROM tblDangKyXoSo(NOLOCK)
	) AS T
	-- Tổng lượt trúng
	OUTER APPLY(
		SELECT COUNT(UserID) AS TongLuotTrung FROM tblDangKyXoSo(NOLOCK) WHERE TrungThuong = 1
	) AS T2
	-- Tổng lượt ko trúng
	OUTER APPLY(
		SELECT COUNT(UserID) AS TongKhongLuotTrung FROM tblDangKyXoSo(NOLOCK) WHERE TrungThuong = -1
	) AS T3
	-- Tổng lượt chưa dò số
	OUTER APPLY(
		SELECT COUNT(UserID) AS TongLuotChuaDo FROM tblDangKyXoSo(NOLOCK) WHERE TrungThuong = 0
	) AS T4
END
GO
