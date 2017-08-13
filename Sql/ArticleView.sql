-- Возвращает статью
if exists(select 1 from sysobjects where name = N'ArticleView' and xtype='P') drop proc ArticleView
go
create proc ArticleView (
	@Id			nvarchar(255),
	@Alias		nvarchar(255)
	) as
begin	
	
	if @Id is not null begin
	
		select 
			a.Id
			,a.Title
			,a.Alias
			,a.SubTitle
			,a.TextPrev
			,a.CategoryId
			,a.IsActive
			,a.MetaTitle
			,a.MetaKeys
			,a.MetaDesc
			,a.MetaNoIndex
			,a.MetaNoFollow
			,a.ChangedAt
			,a.CreatedAt
			,a.UserId
			,a.TextFull
			,a.OtherUser
			,a.PublishedAt
			,a.Hits
			,a.IsCentral
			,a.CentralNumber
			,a.CssId
			,a.CssClasses
			,cat.Name as 'catName'
			,cat.Alias as 'catAlias'
			,cat.IsActive as 'catIsActive'
 
		from Articles a 
			left join Categories cat on a.CategoryId = cat.Id
		where a.Id = @Id
	
	end else begin
	
		select
			a.Id
			,a.Title
			,a.Alias
			,a.SubTitle
			,a.TextPrev
			,a.CategoryId
			,a.IsActive
			,a.MetaTitle
			,a.MetaKeys
			,a.MetaDesc
			,a.MetaNoIndex
			,a.MetaNoFollow
			,a.ChangedAt
			,a.CreatedAt
			,a.UserId
			,a.TextFull
			,a.OtherUser
			,a.PublishedAt
			,a.Hits
			,a.IsCentral
			,a.CentralNumber
			,a.CssId
			,a.CssClasses
			,cat.Name as 'catName'
			,cat.Alias as 'catAlias'
			,cat.IsActive as 'catIsActive'
			 
		from Articles a 
			left join Categories cat on a.CategoryId = cat.Id
		where a.Alias = @Alias
	
	end
	
end
go