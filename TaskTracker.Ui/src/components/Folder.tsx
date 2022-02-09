import React from 'react';
import IFolder from '../interfaces/IFolder';

class Folder extends React.Component<IFolderProps> {
    render() {
        const folder = this.props.folder;
        return (
            <div>
                <ul>
                    <li>
                        {folder.title} (не выполнено: {folder.incompleteTaskCount} задач) (id: {folder.id})
                    </li>
                </ul>
            </div>
        );
    }
}

interface IFolderProps {
    folder: IFolder;
}

export default Folder;
