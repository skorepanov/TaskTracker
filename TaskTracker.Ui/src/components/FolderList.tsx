import React from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Collapse } from 'antd';
import { useStore } from '../stores/RootStore';
import { formatDateTime } from '../utils';
import Task from './Task';

const { Panel } = Collapse;

const FolderList: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    return (
        <Collapse accordion>
            {
                folderStore.folders.map(f => {
                    const incompletedTasks = taskStore.incompletedTasks
                        .filter(t => t.folderId === f.id);

                    const completedTasks = taskStore.completedTasks
                        .filter(t => t.folderId === f.id);

                    const incompleteTaskCountAsText =
                        `(не выполнено: ${incompletedTasks.length} задач)`;

                    const modifiedDateTimeAsText = f.modifiedDateTime !== null
                        ? <><i>Изменена: {formatDateTime(f.modifiedDateTime)}</i><br /></>
                        : null;

                    const handleDeleteButtonClick = async () => {
                        await folderStore.deleteFolder(f);
                    }

                    const header =
                        <>
                            [{f.id}] {f.title} {incompleteTaskCountAsText}<br />
                            {modifiedDateTimeAsText}
                            <i>Создана: {formatDateTime(f.createdDateTime)}</i><br />
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
                                    />)
                            }
                            {
                                completedTasks.map(t =>
                                    <Task
                                        key={t.id}
                                        task={t}
                                    />)
                            }
                        </Panel>
                    );
                })
            }
        </Collapse>
    );
});

export default FolderList;
