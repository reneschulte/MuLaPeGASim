function docChanged(text)
{
	document.writeln(text + this.document.lastModified);
}

function checkTop(doc)
{
	if (doc.self == doc.top) doc.top.location.href = "index.htm";
}