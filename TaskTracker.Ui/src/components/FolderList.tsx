import React from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Collapse } from 'antd';
import { useStore } from '../stores/RootStore';
import { formatDateTime } from '../utils';
import Task from './Task';

const FolderList: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    const folderCollapseItems = folderStore.folders.map(f => {
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

        const incompletedTaskComponents = incompletedTasks.map(t =>
            <Task key={t.id} task={t} />
        );

        const completedTaskComponents = completedTasks.map(t =>
            <Task key={t.id} task={t} />
        );

        const taskComponents =
            <>
                {incompletedTaskComponents}
                {completedTaskComponents}
            </>;

        return {
            key: f.id,
            label: header,
            children: taskComponents
        };
    });

    return (
        <Collapse accordion items={folderCollapseItems} />
    );
});

export default FolderList;
