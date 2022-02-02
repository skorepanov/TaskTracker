import React from 'react';
import IFolder from '../interfaces/IFolder';

class Folder extends React.Component<IFolderProps, IFolderState> {
    constructor(props: IFolderProps) {
        super(props);
        
        this.state = {
            folder: props.folder
        };
    }

    render() {
        const folder = this.state.folder;
        return (
            <div>
                ID: {folder.id}<br />
                Title: {folder.title}<br />
                Incomplete task count: {folder.incompleteTaskCount}
            </div>
        );
    }
}

interface IFolderProps {
    folder: IFolder;
}

interface IFolderState {
    folder: IFolder;
}

export default Folder;
