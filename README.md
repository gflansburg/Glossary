# DNN-Glossary
Glossary Module

Be sure to add "{ name: 'Glossary Item', element: 'span', attributes: { 'class': 'glossaryitem' } }" to your Styles Set in the CKEditor Provider Settings on the Editor Config Tag.  You may want to grab the default styles.js and include those as well, otherwise the 'Glossary Item' style will be the only one in the dropdown list.

You will probably want to include a special style for glossaryitem in your sites Theme CSS as well.  I use this one:

a.glossaryitem {
    color: #000000;
	border-bottom-color: #009966;
    border-bottom-width: 1px;
    border-bottom-style: dotted;
    text-decoration: none !important;
	font-size: 14px;
    font-family: Arial, Helvetica, sans-serif;
}
