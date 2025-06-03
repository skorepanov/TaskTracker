import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskListItem from "./TaskListItem";
import NoTasks from "./NoTasks";
import TaskCreationModalButton from "./TaskCreationModalButton";

const AllTaskList: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    const incompletedTaskComponents = taskStore.incompletedTasks.map(t => (
        <TaskListItem
            key={t.id}
            task={t}
            shouldShowFolder={true}
            folder={folderStore.folders.find(f => f.id === t.folderId)}
        />
    ));

    const completedTaskComponents = taskStore.completedTasks.map(t => (
        <TaskListItem
            key={t.id}
            task={t}
            shouldShowFolder={true}
            folder={folderStore.folders.find(f => f.id === t.folderId)}
        />
    ));

    return (
        <>
            <div style={{ float: "left" }}>
                <strong>Все задачи</strong>
            </div>
            <div style={{ float: "right" }}>
                <TaskCreationModalButton />
            </div>
            <Divider />
            {incompletedTaskComponents.length > 0 ? (
                incompletedTaskComponents
            ) : (
                <NoTasks />
            )}
            {completedTaskComponents.length > 0 ? <Divider /> : null}
            {completedTaskComponents}
        </>
    );
});

export default AllTaskList;
