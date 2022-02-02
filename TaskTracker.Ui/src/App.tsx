import React from 'react';

import { AppUrl, Api } from './api';
import Folder from './components/Folder';
import IFolder from './interfaces/IFolder';

class App extends React.Component<IAppProps, IAppState> {
  constructor(props: IAppProps) {
    super(props);
    
    this.state = {
      folders: [],
    };
  }

  async loadFolders() {
    const url = `${AppUrl}/folders`;
    
    const folders = await Api.get<IFolder[]>(url);
    return this.setState({ folders: folders });
  }

  componentDidMount() {
    this.loadFolders();
  }

  render() {
    return (
      <div>
        <strong>Folders</strong>
        {this.state.folders.map(f =>
            <Folder folder={f} key={f.id} />)}
      </div>
    );
  }
}

interface IAppProps {
}

interface IAppState {
  folders: IFolder[];
}

export default App;
