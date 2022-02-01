import React from 'react';

function App() {
  const folder1 = new Folder(1, "Первая папка");
  const folder2 = new Folder(2, "Вторая папка");
  const folders: Folder[] = [folder1, folder2];

  return (
    <div>
      Folders
      {folders.map(f => <div>ID: {f.id}, Title: {f.title}</div>)}
    </div>
  );
}

class Folder {
  id: Number;
  title: string;

  constructor(id: number, title: string) {
    this.id = id;
    this.title = title;
  }
}

export default App;
