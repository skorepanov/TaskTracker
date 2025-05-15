import React from 'react';
import { Button, Collapse } from 'antd';
import IFolder from '../interfaces/IFolder';
import Task from './Task';
import ITask from "../interfaces/ITask";

const { Panel } = Collapse;

interface IFolderListProps {
    folders: IFolder[];
    incompletedTasks: ITask[];
    completedTasks: ITask[];
    deleteFolder: (folder: IFolder) => Promise<void>;
    completeTask: (task: ITask) => Promise<void>;
    incompleteTask: (task: ITask) => Promise<void>;
    moveTaskToTrash: (task: ITask) => Promise<void>;
}

const FolderList: React.FC<IFolderListProps> = (props) => {
    return (
        <Collapse accordion>
            {
                props.folders.map(f => {
                    const incompletedTasks = props.incompletedTasks
                        .filter(task => task.folderId === f.id);

                    const completedTasks = props.completedTasks
                        .filter(task => task.folderId === f.id);

                    const incompleteTaskCountAsText =
                        `(не выполнено: ${incompletedTasks.length} задач)`;

                    const modifiedDateTimeAsText = f.modifiedDateTime !== null
                        ? <><i>Изменена: {f.modifiedDateTime.toString()}</i><br /></>
                        : null;

                    const handleDeleteButtonClick = async () => {
                        await props.deleteFolder(f);
                    }

                    const header =
                        <>
                            [{f.id}] {f.title} {incompleteTaskCountAsText}<br />
                            {modifiedDateTimeAsText}
                            <i>Создана: {f.createdDateTime.toString()}</i><br />
                            <Button
                                onClick={handleDeleteButtonClick}
                            >
                                Удалить
                            </Button>
                        </>;

                    return (
                        <Panel
                            key={f.id}
                            header={header}
                        >
                            {
                                incompletedTasks.map(t =>
                                    <Task
                                        key={t.id}
                                        task={t}
                                        completeTask={props.completeTask}
                                        incompleteTask={props.incompleteTask}
                                        moveTaskToTrash={props.moveTaskToTrash}
                                    />)
                            }
                            {
                                completedTasks.map(t =>
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
