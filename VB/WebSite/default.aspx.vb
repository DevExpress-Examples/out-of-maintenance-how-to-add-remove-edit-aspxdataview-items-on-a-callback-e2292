Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Web

Partial Public Class _default
	Inherits System.Web.UI.Page
	' Data 
	Public Class MyItem
		Private id_Renamed As String
		Private itemValue_Renamed As Integer

		Public Property Id() As String
			Get
				Return id_Renamed
			End Get
			Set(ByVal value As String)
				id_Renamed = value
			End Set
		End Property
		Public Property ItemValue() As Integer
			Get
				Return itemValue_Renamed
			End Get
			Set(ByVal value As Integer)
				itemValue_Renamed = value
			End Set
		End Property
		Public Sub New()
			Me.New(-1)
		End Sub
		Public Sub New(ByVal itemValue As Integer)
			Me.id_Renamed = Guid.NewGuid().ToString()
			Me.itemValue_Renamed = itemValue
		End Sub
	End Class
	Public Class MyItems
		Inherits List(Of MyItem)

		Public Sub New()
			MyBase.New()
			' Adding sample data
			Add(New MyItem(1))
			Add(New MyItem(2))
			Add(New MyItem(3))
		End Sub
		Public Function NewItem() As MyItem
			Dim item As New MyItem()
			Add(item)
			Return item
		End Function
		Public Sub DeleteById(ByVal id As String)
			For i As Integer = 0 To Count - 1
				Dim item As MyItem = Me(i)
				If item.Id = id Then
					RemoveAt(i)
					Exit For
				End If
			Next i
		End Sub
	End Class

	Public ReadOnly Property Data() As MyItems
		Get
			If Session("MyItems") Is Nothing Then
				Session("MyItems") = New MyItems()
			End If
			Return CType(Session("MyItems"), MyItems)
		End Get
	End Property
	' End Data 

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		dataView.DataSource = Data
		dataView.DataBind()
	End Sub

	Protected Sub dataView_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.CallbackEventArgsBase)
		UpdateMyItemsFromClient()

		If e.Parameter.StartsWith("delete") Then
			Dim parts() As String = e.Parameter.Split(New String() { "=" }, StringSplitOptions.RemoveEmptyEntries)
			Dim id As String = parts(1)
			Data.DeleteById(id)
		Else
			If e.Parameter = "add" Then
				Data.NewItem()
			End If
		End If

		dataView.DataBind()
	End Sub
	Protected Sub btSave_Click(ByVal sender As Object, ByVal e As EventArgs)
		UpdateMyItemsFromClient()
		' Here you can save data to a database
	End Sub

	Protected Function GetDeleteLink(ByVal id As Object) As String
		Dim s As String
		If (id IsNot Nothing) Then
			s = id.ToString()
		Else
			s = ""
		End If
		Return String.Format("javascript:DeleteItem('{0}');", s)
	End Function
	Protected Sub UpdateMyItemsFromClient()
		For i As Integer = 0 To dataView.VisibleItems.Count - 1
			Dim item As DataViewItem = dataView.Items(i)
			Dim comboBox As ASPxComboBox = CType(dataView.FindItemControl("comboItemValue", item), ASPxComboBox)

			Data(i).ItemValue = CInt(Fix(comboBox.Value))
		Next i
	End Sub
End Class
