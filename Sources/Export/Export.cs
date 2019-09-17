using System;
using System.Data;
using System.IO;
using HandlebarsDotNet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using Orpea_WF.Resident;

namespace Orpea_WF
{
    public class Export
    {
        public static DataTable DtCentresInteret      { private get; set; }
        public static string NomResident              { private get; set; }
        public static string PrenomResident           { private get; set; }
        public static string NaissanceResident        { private get; set; }
        public static string LieuNaissanceResident    { private get; set; }
        public static string StatutResident           { private get; set; }
        public static int NbrEnfantResident           { private get; set; }
        public static int ChambreResident             { private get; set; }
        public static string AutonomieResident        { private get; set; }
        public static DateTime DateEntreeResident     { private get; set; }
        public static string MetierResident           { private get; set; }
        public static string HabitatAnterieurResident { private get; set; }


        public static void Planning(DataTable dtblTable, string strPdfPath, string strHeader)
        {
            var fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            var document = new Document();
            document.SetPageSize(PageSize.A4);
            var writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Report Header
            var bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            var fntHead = new Font(bfntHead, 16, 1, BaseColor.BLACK);
            var prgHeading = new Paragraph { Alignment = Element.ALIGN_CENTER };
            prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
            document.Add(prgHeading);

            //Author
            var prgAuthor = new Paragraph();
            var btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            var fntAuthor = new Font(btnAuthor, 8, 2, BaseColor.BLACK);
            prgAuthor.Alignment = Element.ALIGN_RIGHT;
            prgAuthor.Add(new Chunk($"Utilisateur : {frmConnexion.ConnexionResident.IdUtilisateur}", fntAuthor));
            prgAuthor.Add(new Chunk($"\nImprimé le : {DateTime.Now.ToShortDateString()}", fntAuthor));
            document.Add(prgAuthor);

            //Add a line seperation
            var p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 80.0F, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n", fntHead));

            //Write the table
            var table = new PdfPTable(dtblTable.Columns.Count);
            
            //Table header
            var btnColumnHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            var fntColumnHeader = new Font(btnColumnHeader, 10, 1, BaseColor.WHITE);
            for (var i = 0; i < dtblTable.Columns.Count; i++)
            {
                var cell = new PdfPCell { BackgroundColor = BaseColor.DARK_GRAY, HorizontalAlignment = Element.ALIGN_CENTER };
                cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                table.AddCell(cell);
            }
            //table Data
            for (var i = 0; i < dtblTable.Rows.Count; i++)
            {
                for (var j = 0; j < dtblTable.Columns.Count; j++)
                {
                    table.AddCell(dtblTable.Rows[i][j].ToString());
                }
            }

            document.Add(table);
            document.Close();
            writer.Close();
            fs.Close();
        }

        private static string ConvertDataTableToHtml(DataTable dt)
        {
            var html = "<table align=\"center\" id=\"customers\">";

            html += "<tr>";
            for (var i = 0; i < dt.Columns.Count; i++)
                html += "<td width=\"300\" align=\"center\"><b>" + dt.Columns[i].ColumnName + "</b</td>";
            html += "</tr>";

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (var j = 0; j < dt.Columns.Count; j++)
                    html += "<td align=\"center\">" + dt.Rows[i][j] + "</td>";
                html += "</tr>";
            }
            html += "</table>";

            return html;
        }

        public static DataTable DataTable()
        {
            using (var mySqlConnection = new MySqlConnection())
            {
                mySqlConnection.ConnectionString = @"server=mysql-orpeafauriel.alwaysdata.net;Port=3306; userid=180881; password=Rdc3ycww**; database=orpeafauriel_bdd";
                mySqlConnection.Open();

                const string query = @"SELECT RESID_NOM, CAT_LIBELLE, ACT_LIBELLE, COTA_LIBELLE
                                       FROM CENTRES_INTERET CI
                                       INNER JOIN RESIDENTS R ON CI.CI_RESID = R.RESID_ID
                                       INNER JOIN CATEGORIES C ON CI.CI_CATEG = C.CAT_CODE
                                       INNER JOIN ACTIVITES A ON CI.CI_ACT = A.ACT_CODE
                                       INNER JOIN COTATIONS CO ON CI.CI_COTATION = CO.COTA_ID
                                       WHERE CI.CI_RESID = @RESID_ID ORDER BY CAT_LIBELLE ASC";

                using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                {
                    mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_ID", MySqlDbType.Int16));
                    mySqlCommand.Parameters["@RESID_ID"].Value = FicheResident.IdResident;

                    using (var adapter = new MySqlDataAdapter(mySqlCommand))
                    {
                        adapter.Fill(DtCentresInteret);

                        DtCentresInteret.Columns["RESID_NOM"].ColumnName    = "NOM";
                        DtCentresInteret.Columns["CAT_LIBELLE"].ColumnName  = "CATÉGORIE";
                        DtCentresInteret.Columns["ACT_LIBELLE"].ColumnName  = "ACTIVITÉ";
                        DtCentresInteret.Columns["COTA_LIBELLE"].ColumnName = "COTATION";
                        DtCentresInteret.AcceptChanges();
                        return DtCentresInteret;
                    }
                }
            }
        }

        public static void FicheResidentHtmlCss()
        {
            var url = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var source =
                @"<html><head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
<meta name=""Generator"" content=""Microsoft Word 15 (filtered)"">
<style>
<!--
 /* Font Definitions */
 @font-face
	{font-family:Wingdings;
	panose-1:5 0 0 0 0 0 0 0 0 0;}
@font-face
	{font-family:""Cambria Math"";
	panose-1:2 4 5 3 5 4 6 3 2 4;}
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin:0cm;
	margin-bottom:.0001pt;
	text-autospace:none;
	font-size:11.0pt;
	font-family:""Arial"",sans-serif;}
h1
	{margin-top:4.5pt;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:0;
	margin-bottom:.0001pt;
	text-autospace:none;
	font-size:16.0pt;
	font-family:""Arial"",sans-serif;}
td
    {font-family:""Arial"",sans-serif;}
h2
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:143.9pt;
	margin-bottom:.0001pt;
	text-autospace:none;
	font-size:14.0pt;
	font-family:""Arial"",sans-serif;}
h3
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:26.25pt;
	margin-bottom:.0001pt;
	text-autospace:none;
	font-size:12.0pt;
	font-family:""Arial"",sans-serif;}
p.MsoHeader, li.MsoHeader, div.MsoHeader
	{mso-style-link:""En-tête Car"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-autospace:none;
	font-size:11.0pt;
	font-family:""Arial"",sans-serif;}
p.MsoFooter, li.MsoFooter, div.MsoFooter
	{mso-style-link:""Pied de page Car"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-autospace:none;
	font-size:11.0pt;
	font-family:""Arial"",sans-serif;}
p.MsoBodyText, li.MsoBodyText, div.MsoBodyText
	{margin:0cm;
	margin-bottom:.0001pt;
	text-autospace:none;
	font-size:12.0pt;
	font-family:""Arial"",sans-serif;}
p.MsoListParagraph, li.MsoListParagraph, div.MsoListParagraph
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:61.7pt;
	margin-bottom:.0001pt;
	text-indent:-18.0pt;
	text-autospace:none;
	font-size:11.0pt;
	font-family:""Arial"",sans-serif;}
p.TableParagraph, li.TableParagraph, div.TableParagraph
	{mso-style-name:""Table Paragraph"";
	margin:0cm;
	margin-bottom:.0001pt;
	text-autospace:none;
	font-size:11.0pt;
	font-family:""Arial"",sans-serif;}
span.En-tteCar
	{mso-style-name:""En-tête Car"";
	mso-style-link:En-tête;
	font-family:""Arial"",sans-serif;}
span.PieddepageCar
	{mso-style-name:""Pied de page Car"";
	mso-style-link:""Pied de page"";
	font-family:""Arial"",sans-serif;}
.MsoChpDefault
	{font-family:""Calibri"",sans-serif;}
.MsoPapDefault
	{text-autospace:none;}
 /* Page Definitions */
 @page WordSection1
	{size:595.0pt 842.0pt;
	margin:19.0pt 9.0pt 45.0pt 22.0pt;}
div.WordSection1
	{page:WordSection1;}
@page WordSection2
	{size:595.0pt 842.0pt;
	margin:20.0pt 9.0pt 45.0pt 22.0pt;}
div.WordSection2
	{page:WordSection2;}
 /* List Definitions */
 ol
	{margin-bottom:0cm;}
ul
	{margin-bottom:0cm;}


#customers {
  font-family: ""Trebuchet MS"", Arial, Helvetica, sans-serif;
  border-collapse: collapse;
  width: 100%;
}

#customers td, #customers th {
  border: 1px solid #ddd;
  padding: 8px;
}

#customers tr:nth-child(even){background-color: #f2f2f2;}

#customers tr:hover {background-color: #ddd;}

#customers th {
  padding-top: 12px;
  padding-bottom: 12px;
  text-align: left;
  background-color: #4CAF50;
  color: white;
}
-->
</style>

</head>

<body lang=""FR"">
<div style=""width:660px;height:35px;border:1px solid #000;margin:0 auto;text-align:center;""><h1>FICHE ADMINISTRATIVE D'ADMISSION</h1></div>
<br>
<br>
<div class=""WordSection1"">

<p class=""MsoBodyText"" style=""margin-top:.35pt""><span style=""font-size:6.5pt"" lang=""EN-US"">&nbsp;</span></p>

<table class=""TableNormal"" style=""margin-left:
 20.6pt;border-collapse:collapse;border:none"" cellspacing=""0"" cellpadding=""0"" border=""1"">
 <tbody><tr style=""height:16.1pt"">
  <td style=""width:509.2pt;border:solid black 1.0pt;
  background:#BFBFBF;padding:0cm 0cm 0cm 0cm;height:16.1pt"" width=""679"" valign=""top"">
  <p class=""TableParagraph"" style=""margin-top:0cm;margin-right:144.95pt;
  margin-bottom:0cm;margin-left:145.35pt;margin-bottom:.0001pt;text-align:center;
  line-height:15.1pt"" align=""center""><b><span style=""font-size:14.0pt"" lang=""EN-US"">IDENTITE DU
  RESIDENT</span></b></p>
  </td>
 </tr>
 <tr style=""height:129.85pt"">
  <td style=""width:509.2pt;border:solid black 1.0pt;
  border-top:none;padding:0cm 0cm 0cm 0cm;height:129.85pt"" width=""679"" valign=""top"">
  <p class=""TableParagraph"" style=""margin-top:9.15pt;margin-right:9.7pt;
  margin-bottom:0cm;margin-left:5.45pt;margin-bottom:.0001pt;text-align:justify""><span style=""font-size:12.0pt"" lang=""EN-US"">Nom : <b>{{nom}}</b> Prénom :
   <b>{{prenom}}</b> </span></p>
  <p class=""TableParagraph"" style=""margin-top:9.15pt;margin-right:9.7pt;
  margin-bottom:0cm;margin-left:5.45pt;margin-bottom:.0001pt;text-align:justify""><span style=""font-size:12.0pt"" lang=""EN-US"">Date de naissance : <b>{{dateNaissance}}</b> Lieu de
  naissance : <b>{{lieuNaissance}}</b> </span></p>
  <p class=""TableParagraph"" style=""margin-top:9.15pt;margin-right:9.7pt;
  margin-bottom:0cm;margin-left:5.45pt;margin-bottom:.0001pt;text-align:justify""><span style=""font-size:12.0pt"" lang=""EN-US"">Statut : <b>{{statut}}</b> 
  Enfant(s) : <b>{{nombreEnfant}}</b> Chambre : <b>{{numeroChambre}}</b> Autonomie : <b>{{autonomie}}</b> Date entrée dans la </span></p>
  <p class=""TableParagraph"" style=""margin-top:9.15pt;margin-right:9.7pt;
  margin-bottom:0cm;margin-left:5.45pt;margin-bottom:.0001pt;text-align:justify""><span style=""font-size:12.0pt"" lang=""EN-US"">résidence : <b>{{dateEntree}}</b> Date de réalisation
  de la fiche : <b>{{dateCreationFiche}}</b> </span></p>
  <p class=""TableParagraph"" style=""margin-top:9.15pt;margin-right:9.7pt;
  margin-bottom:0cm;margin-left:5.45pt;margin-bottom:.0001pt;text-align:justify""><span style=""font-size:12.0pt"" lang=""EN-US"">Profession(s) antérieurement exercée(s) :
   <b>{{metierExerce}}</b> </span></p>
  <p class=""TableParagraph"" style=""margin-top:9.15pt;margin-right:9.7pt;
  margin-bottom:0cm;margin-left:5.45pt;margin-bottom:.0001pt;text-align:justify""><span style=""font-size:12.0pt"" lang=""EN-US"">Habitat antérieur :
   <b>{{typeHabitatAnterieur}}</b> </span></p>
  <p class=""TableParagraph"" style=""margin-left:5.45pt;text-align:justify""><span style=""font-size:12.0pt"" lang=""EN-US"">&nbsp;</span></p>
  </td>
 </tr>  
 <tr style=""height:16.05pt"">
  <td style=""width:509.2pt;border:solid black 1.0pt;
  border-top:none;background:#BFBFBF;padding:0cm 0cm 0cm 0cm;height:16.05pt"" width=""679"" valign=""top"">
  <p class=""TableParagraph"" style=""margin-top:.05pt;margin-right:
  144.85pt;margin-bottom:0cm;margin-left:145.45pt;margin-bottom:.0001pt;
  text-align:center;line-height:15.05pt"" align=""center""><b><span style=""font-size:
  14.0pt"" lang=""EN-US"">ACTIVITÉS PRATIQUÉES</span></b></p>
  </td>
 </tr>
 <tr style=""height:66.25pt"">
  <td style=""width:509.2pt;border:solid black 1.0pt;
  border-top:none;padding:0cm 0cm 0cm 0cm;height:66.25pt"" width=""679"" valign=""top"">

 <br>" + ConvertDataTableToHtml(DtCentresInteret) + @"</td>
 </tr>
</tbody></table>

</div>

<span style=""font-size:12.0pt;font-family:&quot;Arial&quot;,sans-serif""><br style=""page-break-before:always"" clear=""all"">
</span>

<div class=""WordSection2"">
</div>
</body>
</html>";


            var template = Handlebars.Compile(source);
            var data = new
            {
                nom                  = NomResident,
                prenom               = PrenomResident,
                dateNaissance        = NaissanceResident,
                lieuNaissance        = LieuNaissanceResident.ToUpper(),
                statut               = StatutResident.ToUpper(),
                nombreEnfant         = NbrEnfantResident,
                numeroChambre        = ChambreResident,
                dateEntree           = DateEntreeResident,
                dateCreationFiche    = DateTime.Now.ToString("dd/MM/yyyy").ToUpper(),
                metierExerce         = MetierResident,
                typeHabitatAnterieur = HabitatAnterieurResident,
                autonomie            = AutonomieResident
            };
            var result = template(data);

            var renderer = new IronPdf.HtmlToPdf
            {
                PrintOptions =
                {
                    FirstPageNumber = 1,
                    Header          = {DrawDividerLine = true, CenterText = "{url}", FontFamily = "Helvetica,Arial", FontSize = 12},
                    Footer          =
                    {
                        DrawDividerLine = true,
                        FontFamily      = "Arial",
                        FontSize        = 10,
                        LeftText        = "{date} {time}",
                        RightText       = "{page} / {total-pages}"
                    }
                }
            };

            renderer.RenderHtmlAsPdf(result).SaveAs($@"{url}\Orpea_Gestion\Fiche_résident\{data.nom}.pdf");
        }
    }
}
