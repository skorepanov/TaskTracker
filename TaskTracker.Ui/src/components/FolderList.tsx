import React from 'react';
import { Collapse } from 'antd';
import IFolder from '../interfaces/IFolder';
import Task from './Task';
import ITask from "../interfaces/ITask";

const { Panel } = Collapse;

class FolderList extends React.Component<IFolderListProps> {
    onCollapseChange = (folderIds: string | string[] | undefined) => {
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
            <Collapse
                accordion
                onChange={this.onCollapseChange}
            >
            {
                this.props.folders.map(f => {
                    const header = `[${f.id}] ${f.title} (не выполнено: ${f.incompleteTaskCount} задач)`;
                    return (
                        <Panel
                            key={f.id}
                            header={header}
                        >
                        {
                            f.tasks?.map(t =>
                                <Task
                                    key={t.id}
                                    task={t}
                                    completeTask={this.props.completeTask}
                                    incompleteTask={this.props.incompleteTask}
                                />)
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
    completeTask: (task: ITask) => Promise<void>;
    incompleteTask: (task: ITask) => Promise<void>;
}

export default FolderList;
