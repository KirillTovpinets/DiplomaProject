SET IDENTITY_INSERT [dbo].[Table] ON
INSERT INTO [dbo].[Table] ([Id],[Login],[Password],[Name],[Sirname],[Patername],[BirthYear],[Photo]) 
SELECT 2, 'Kirill','maleheslo1234','Кирилл','Товпинец','Александрович',1994, BulkColumn 
FROM Openrowset( Bulk '\\psf\Home\Documents\Visual Studio 2013\Projects\HtmlInputs\HtmlInputs\Content\images\Kirill.jpg', Single_Blob) AS image
SET IDENTITY_INSERT [dbo].[Table] OFF