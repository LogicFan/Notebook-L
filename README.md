# Notebook-L

## Design

The UML diagram is shown at [here](./Documentation/build/UML.pdf).

## Notebooks

There is a default (local) notebook, which is located in `$(ApplicationData.Current.LocalFolder)/Notebook-L/**`. For each web drive account, the notebook location is `/Notebook-L/**`.

Any folder with name of format `**.nbl` is considered as a document, anything else is considered as a regular folder.

Here is an example of the structure of the notebook

```
Notebook-L
 |--A           (Folder)
 |   |--B       (Folder)
 |   |--X.nbl   (Document)
 |--Y.nbl       (Document)
```

## Documents

Each document consist the following components,
- Text Document
- Audio Recording
- Search Index

### Text Document

A document can consist only a single text document. The text document has 4 layers, ordered from top to down
1. Ink drawing.
2. Text, image, LaTex Formula, Table, etc.
3. Text constraint layer (invisible)
4. PDF background

### Audio Recording

A document may consist multiple audio recording. Each recording has the `.wav` audio file and a time-sync metadata.

### Search Index

The search index is used for searching purpose. It will be used for search purpose only. It consist text, image OCR, ink OCR and audio StoT.

A structure of document in file system will be
```
A.nbl
 |--background.pdf  (PDF background)
 |--document.json   (Text constraint layer, text, LaTex Formula, Table, and other metadata)
 |--Image           (Folder)
 |   |--1.png       (Image)
 |   |--2.png       (Image)
 |--Drawing         (Folder)
 |   |--1.isf       (Drawing Image)
 |   |--2.isf       (Drawing Image)
 |--Recording       (Folder)
 |   |--1.wav       (Audio File)
 |   |--1.json      (Time-sync File)
 |--search.json     (Search Index)
```

## Template

A template is a single page PDF file plus text constraint layer. So it consist two files in a `.zip` file

```
X.zip
 |--X.pdf
 |--X.json
```

## Attribute

- [Icons](./Notebook%20L/Assets/Local.svg) made by <a href="https://www.flaticon.com/authors/freepik" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon"> www.flaticon.com</a>