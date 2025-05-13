import React from 'react';
import { Collapse } from 'antd';
import IFolder from '../interfaces/IFolder';
import Task from './Task';
import ITask from "../interfaces/ITask";

const { Panel } = Collapse;

interface IFolderListProps {
    folders: IFolder[];
    loadTasks: (folderId: number) => Promise<void>;
    completeTask: (task: ITask) => Promise<void>;
    incompleteTask: (task: ITask) => Promise<void>;
    moveTaskToTrash: (task: ITask) => Promise<void>;
}

const FolderList: React.FC<IFolderListProps> = (props) => {
    const handleCollapseChange = (folderIds: string[]) => {
        if (folderIds.length === 0) {
            return;
        }

        const folderId = Array.isArray(folderIds)
            ? folderIds[0]
            : folderIds;

        const parsedFolderId = Number.parseInt(folderId);
        props.loadTasks(parsedFolderId);
    }

    return (
        <Collapse
            accordion
            onChange={handleCollapseChange}
        >
            {
                props.folders.map(f => {
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
                                        completeTask={props.completeTask}
                                        incompleteTask={props.incompleteTask}
                                        moveTaskToTrash={props.moveTaskToTrash}
                                    />)
                            }
                        </Panel>
                    );
                })
            }
        </Collapse>
    );
}

export default FolderList;
