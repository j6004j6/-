
-- �������and �Ƨ�����
with T1 as(
select *,ROW_NUMBER() over(partition by item_no order by updatetime desc) noUP from Item_Standard_ITP_Master
where year=2023 and Quarter=4 
)
select * from T1
where noUP =1
order by UpdateTime desc

--��0���g�k RIGHT => �S��11 �b�Ƹ�����[3��0 �[��11�X => �̤֮Ƹ��ƭn��8�X�A�p�G�W�L11�X������11�X
select *,RIGHT('000'+CAST(Item_No as nvarchar),11) as test from Item_Standard_ITP_Master
where year=2023 and Quarter=4

--�]�i�H�Ψ�update���y�k�W
update Item_Standard_ITP_Master set Item_No=RIGHT('000'+CAST(Item_No as nvarchar),11)

--���� replace�y�k +update
update T_Menu set webUrl=REPLACE(webUrl,'A','B')
where menuContent like '%http://pubweb.abe.tw%'


--�f�t������檺Update 
With DataA as(
  select * from ptpay P
  join nh_name nh on nh.IDNO=P.sernum and p.id='12345'
)
update nh_name set K_name=DataA.Name
from DataA
where DataA.sernum=nh_name.IDNO
