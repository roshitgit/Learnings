Different ways how to escape an XML string in C#
XML encoding is necessary if you have to save XML text in an XML document. If you don't escape special chars the XML to insert will become a part of the original XML DOM and not a value of a node.

Escaping the XML means basically replacing 5 chars with new values.

These replacements are:

<	->	&lt;
>	->	&gt;
"	->	&quot;
'	->	&apos;
&	->	&amp;
 

Here are 4 ways you can encode XML in C#:

1. string.Replace() 5 times

This is ugly but it works. Note that Replace("&", "&amp;") has to be the first replace so we don't replace other already escaped &.

string xml = "<node>it's my \"node\" & i like it<node>";
encodedXml = xml.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");

// RESULT: &lt;node&gt;it&apos;s my &quot;node&quot; &amp; i like it&lt;node&gt;
 

2. System.Web.HttpUtility.HtmlEncode()

Used for encoding HTML, but HTML is a form of XML so we can use that too. Mostly used in ASP.NET apps. Note that HtmlEncode does NOT encode apostrophes ( ' ).

string xml = "<node>it's my \"node\" & i like it<node>";
string encodedXml = HttpUtility.HtmlEncode(xml);

// RESULT: &lt;node&gt;it's my &quot;node&quot; &amp; i like it&lt;node&gt;
 

3. System.Security.SecurityElement.Escape()

In Windows Forms or Console apps I use this method. If nothing else it saves me including the System.Web reference in my projects and it encodes all 5 chars.

string xml = "<node>it's my \"node\" & i like it<node>";
string encodedXml = System.Security.SecurityElement.Escape(xml);

// RESULT: &lt;node&gt;it&apos;s my &quot;node&quot; &amp; i like it&lt;node&gt;
 

4. System.Xml.XmlTextWriter

Using XmlTextWriter you don't have to worry about escaping anything since it escapes the chars where needed. For example in the attributes it doesn't escape apostrophes, while in node values it doesn't escape apostrophes and qoutes.

string xml = "<node>it's my \"node\" & i like it<node>";
using (XmlTextWriter xtw = new XmlTextWriter(@"c:\xmlTest.xml", Encoding.Unicode))
{
    xtw.WriteStartElement("xmlEncodeTest");
    xtw.WriteAttributeString("testAttribute", xml);
    xtw.WriteString(xml);
    xtw.WriteEndElement();
}

// RESULT:
/*
<xmlEncodeTest testAttribute="&lt;node&gt;it's my &quot;node&quot; &amp; i like it&lt;node&gt;">
    &lt;node&gt;it's my "node" &amp; i like it&lt;node&gt;
</xmlEncodeTest>
*/
 

Each of the four ways is different, so use each one where you fell appropriate. You can't go wrong with SecurityElement though. :)
