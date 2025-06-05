import React from "react";
import { observer } from "mobx-react-lite";
import { Collapse } from "antd";
import type { CollapseProps } from "antd";
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

    const completedTaskItems: CollapseProps["items"] = [
        {
            key: "completedTasks",
            label: "Выполнено",
            children: completedTaskComponents,
        },
    ];

    const completedTasksPanel =
        completedTaskComponents.length > 0 ? (
            <Collapse
                items={completedTaskItems}
                ghost
                defaultActiveKey={["completedTasks"]}
            />
        ) : null;

    return (
        <>
            {incompletedTaskComponents.length > 0 ? (
                incompletedTaskComponents
            ) : (
                <NoTasks />
            )}
            {completedTasksPanel}
        </>
    );
});

export default TaskList;
