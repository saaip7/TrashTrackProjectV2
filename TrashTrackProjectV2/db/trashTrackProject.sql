USE [master]
GO
/****** Object:  Database [trashTrackProject]    Script Date: 19/12/23 1:34:02 AM ******/
CREATE DATABASE [trashTrackProject]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'trashTrackProject', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS01\MSSQL\DATA\trashTrackProject.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'trashTrackProject_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS01\MSSQL\DATA\trashTrackProject_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [trashTrackProject] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [trashTrackProject].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [trashTrackProject] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [trashTrackProject] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [trashTrackProject] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [trashTrackProject] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [trashTrackProject] SET ARITHABORT OFF 
GO
ALTER DATABASE [trashTrackProject] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [trashTrackProject] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [trashTrackProject] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [trashTrackProject] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [trashTrackProject] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [trashTrackProject] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [trashTrackProject] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [trashTrackProject] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [trashTrackProject] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [trashTrackProject] SET  DISABLE_BROKER 
GO
ALTER DATABASE [trashTrackProject] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [trashTrackProject] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [trashTrackProject] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [trashTrackProject] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [trashTrackProject] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [trashTrackProject] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [trashTrackProject] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [trashTrackProject] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [trashTrackProject] SET  MULTI_USER 
GO
ALTER DATABASE [trashTrackProject] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [trashTrackProject] SET DB_CHAINING OFF 
GO
ALTER DATABASE [trashTrackProject] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [trashTrackProject] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [trashTrackProject] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [trashTrackProject] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [trashTrackProject] SET QUERY_STORE = OFF
GO
USE [trashTrackProject]
GO
/****** Object:  Table [dbo].[tb_subscription]    Script Date: 19/12/23 1:34:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_subscription](
	[SubscriptionID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Voucher] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[SubscriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_user]    Script Date: 19/12/23 1:34:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_user](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](20) NULL,
	[nama] [varchar](20) NULL,
	[email] [varchar](50) NULL,
	[password] [varchar](20) NULL,
	[address] [text] NULL,
	[noTelp] [varchar](22) NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tb_subscription] ON 

INSERT [dbo].[tb_subscription] ([SubscriptionID], [UserID], [Voucher]) VALUES (1, 3, 66)
INSERT [dbo].[tb_subscription] ([SubscriptionID], [UserID], [Voucher]) VALUES (2, 9, 8)
SET IDENTITY_INSERT [dbo].[tb_subscription] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_user] ON 

INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (3, N'admin', N'admin1', N'adminsaja', N'admin', N'Babakan, Poncosari', N'085712974454')
INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (6, N'aku', N'aku', N'aku@mail', N'aku', N'085712974454', N'babakan')
INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (7, N'dia', N'dia', N'dia@mail', N'dia', N'085712974454', N'babakan')
INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (8, N'suharti', N'txtNama', N'akuu@mail.com', N'12345', N'bbkn', N'087')
INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (9, N'bagas', N'bagas31', N'bagas@mail.com', N'1234', N'bbk', N'1234')
INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (10, N'jkw', N'jkw', N'jkw', N'jkw', N'solo', N'087')
INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (11, N'jk', N'jk', N'jk', N'jk', N'jk', N'jk')
INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (12, N'anggito', N'anggito', N'anggito', N'anggito', N'anggito', N'anggito')
INSERT [dbo].[tb_user] ([user_id], [username], [nama], [email], [password], [address], [noTelp]) VALUES (13, N'abc', N'abc', N'abc', N'abc', N'abc', N'abc')
SET IDENTITY_INSERT [dbo].[tb_user] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tb_user__AB6E6164E459BAF4]    Script Date: 19/12/23 1:34:02 AM ******/
ALTER TABLE [dbo].[tb_user] ADD UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tb_user__F3DBC572D64ED7F1]    Script Date: 19/12/23 1:34:02 AM ******/
ALTER TABLE [dbo].[tb_user] ADD UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tb_subscription]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[tb_user] ([user_id])
GO
USE [master]
GO
ALTER DATABASE [trashTrackProject] SET  READ_WRITE 
GO
