
-- 虛擬表格and 排序應用
with T1 as(
select *,ROW_NUMBER() over(partition by item_no order by updatetime desc) noUP from Item_Standard_ITP_Master
where year=2023 and Quarter=4 
)
select * from T1
where noUP =1
order by UpdateTime desc

--補0的寫法 RIGHT => 沒滿11 在料號左邊加3個0 加到11碼 => 最少料號數要有8碼，如果超過11碼的取到11碼
select *,RIGHT('000'+CAST(Item_No as nvarchar),11) as test from Item_Standard_ITP_Master
where year=2023 and Quarter=4

--也可以用到update的語法上
update Item_Standard_ITP_Master set Item_No=RIGHT('000'+CAST(Item_No as nvarchar),11)

--延伸 replace語法 +update
update T_Menu set webUrl=REPLACE(webUrl,'A','B')
where menuContent like '%http://pubweb.abe.tw%'


--搭配虛擬表格的Update 
With DataA as(
  select * from ptpay P
  join nh_name nh on nh.IDNO=P.sernum and p.id='12345'
)
update nh_name set K_name=DataA.Name
from DataA
where DataA.sernum=nh_name.IDNO
