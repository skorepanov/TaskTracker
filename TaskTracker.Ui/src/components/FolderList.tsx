import React from 'react';
import { Collapse } from 'antd';
import IFolder from '../interfaces/IFolder';
import Task from './Task';

const { Panel } = Collapse;

class FolderList extends React.Component<IFolderListProps> {
    onCollapseChange(folderIds: string | string[] | undefined) {
        if (folderIds == null || folderIds.length === 0) {
            return;
        }

        const folderId = Array.isArray(folderIds)
            ? folderIds[0]
            : folderIds;

        const parsedFolderId = Number.parseInt(folderId);
        this.props.loadTasks(parsedFolderId);
    }

    render() {
        return (
            <Collapse accordion onChange={ids => this.onCollapseChange(ids)}>
            {
                this.props.folders.map(f => {
                    const header = `[${f.id}] ${f.title} (не выполнено: ${f.incompleteTaskCount} задач)`;
                    return (
                        <Panel header={header} key={f.id}>
                        {
                            f.tasks?.map(t =>
                                <Task task={t} key={t.id}></Task>)
                        }
                        </Panel>
                    );
                })
            }
            </Collapse>
        );
    }
}

interface IFolderListProps {
    folders: IFolder[];
    loadTasks: (folderId: number) => Promise<void>;
}

export default FolderList;
