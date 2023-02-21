# Test
delete from NormalMember where fid in(select max(fid) from NormalMember)
delete from BusinessMember where fid in(select max(fid) from BusinessMember)

delete from Products where B_fid in(select max(fid) from BusinessMember)
delete from ProductCategory where B_fid in(select max(fid) from BusinessMember)
delete from ProductOptions where B_fid in(select max(fid) from BusinessMember)
delete from ProductOptionGroup where B_fid in(select max(fid) from BusinessMember)


update adminMember set [password]='aDmin145' where account='admin'
update normalmember set isSuspensed=1 where fid=2
