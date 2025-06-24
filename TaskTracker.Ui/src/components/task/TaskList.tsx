import React from "react";
import { observer } from "mobx-react-lite";
import { Collapse } from "antd";
import type { CollapseProps } from "antd";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";
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
    const t = useTranslation();

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
            label: (
                <>
                    {t("completedTasks")}
                    <span style={{ color: "grey", marginLeft: 10 }}>
                        {props.completedTasks.length}
                    </span>
                </>
            ),
            children: completedTaskComponents,
        },
    ];

    const completedTasksPanel =
        completedTaskComponents.length > 0 ? (
            <Collapse
                items={completedTaskItems}
                defaultActiveKey={["completedTasks"]}
                ghost
                size={"small"}
                style={{ marginTop: 10 }}
            />
        ) : null;

    return (
        <div className="scrollable-container">
            {incompletedTaskComponents.length > 0 ? (
                <div style={{ paddingLeft: 10 }}>
                    {incompletedTaskComponents}
                </div>
            ) : (
                <NoTasks />
            )}
            {completedTasksPanel}
        </div>
    );
});

export default TaskList;
