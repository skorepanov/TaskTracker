import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import ITask from "../../interfaces/ITask";
import TaskListItem from "./TaskListItem";
import NoTasks from "./NoTasks";

interface ITaskListProps {
    incompletedTasks: ITask[];
    completedTasks: ITask[];
    shouldShowFolder?: boolean;
}

const TaskList: React.FC<ITaskListProps> = observer(props => {
    const { folderStore } = useStore();

    const incompletedTaskComponents = props.incompletedTasks.map(t => (
        <TaskListItem
            key={t.id}
            task={t}
            shouldShowFolder={props.shouldShowFolder}
            folder={
                props.shouldShowFolder
                    ? folderStore.folders.find(f => f.id === t.folderId)
                    : undefined
            }
        />
    ));

    const completedTaskComponents = props.completedTasks.map(t => (
        <TaskListItem
            key={t.id}
            task={t}
            shouldShowFolder={props.shouldShowFolder}
            folder={
                props.shouldShowFolder
                    ? folderStore.folders.find(f => f.id === t.folderId)
                    : undefined
            }
        />
    ));

    return (
        <>
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

export default TaskList;
