import React, { Activity, useState } from "react";
import { Link } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Checkbox, Divider, Dropdown, MenuProps } from "antd";
import { useStore } from "../../stores/RootStore";
import { formatDate } from "../../utils/dateTimeFormatter";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import { useSettings } from "../../contexts/SettingsContext";
import { useTranslation } from "../../hooks/useTranslation";
import ITask from "../../interfaces/ITask";
import IFolder from "../../interfaces/IFolder";
import TaskTag from "../tag/TaskTag";

type MenuItem = Required<MenuProps>["items"][number];

interface ITaskListItemProps {
    task: ITask;
    shouldShowFolder?: boolean;
    folder?: IFolder;
}

const TaskListItem: React.FC<ITaskListItemProps> = observer(props => {
    const { taskStore, tagStore } = useStore();
    const { settings } = useSettings();
    const t = useTranslation();

    const { task } = props;

    const [isCompleted, setIsCompleted] = useState<boolean>(
        task.completedDateTime !== null
    );

    const taskTextColor = isCompleted ? "green" : "";

    const selectedTaskColor = settings.theme === "dark" ? "#111a2c" : "#e6f4ff";
    const backgroundColor =
        taskStore.currentTask?.id === task.id ? selectedTaskColor : "";

    const overdueComponentColor =
        !isCompleted && task.overdueDaysCount > 0 ? "red" : "";

    const overdueDaysComponent =
        <Activity mode={task.overdueDaysCount > 0 ? "visible" : "hidden"}>
            ({task.overdueDaysCount} {t("daysAgo")})
        </Activity>;

    const overdueComponent =
        <Activity mode={task.dueDateTime !== null ? "visible" : "hidden"}>
            <span style={{ color: overdueComponentColor }}>
                {formatDate(task.dueDateTime!)} {overdueDaysComponent}
            </span>
        </Activity>;

    const taskTags = tagStore.getFilteredSortedTags(task.tagIds);

    const taskTagComponents = taskTags
        ? taskTags.map(t => (
              <TaskTag
                  key={t.id}
                  tag={t}
              />
          ))
        : [];

    const handleTaskClick = () => {
        taskStore.currentTaskId = task.id;
    };

    const handleTaskDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        const transferData = JSON.stringify({ taskId: task.id });
        event.dataTransfer.setData("text/plain", transferData);
    }

    const handleCompletedChange = async (event: CheckboxChangeEvent) => {
        const isCompletedNew = event.target.checked;

        setIsCompleted(isCompletedNew);

        if (isCompletedNew) {
            await taskStore.completeTask(task.id);
        } else {
            await taskStore.incompleteTask(task.id);
        }
    };

    const handleMoveTaskToTrashButtonClick = async () => {
        await taskStore.moveTaskToTrash(task.id);
    };

    const contextMenuItems: MenuItem[] = [
        {
            key: "moveTaskToTrash",
            label: t("moveTaskToTrash"),
            onClick: handleMoveTaskToTrashButtonClick,
        },
    ];

    return (
        <>
            <Dropdown
                key={task.id}
                menu={{ items: contextMenuItems }}
                trigger={["contextMenu"]}>
                <div
                    onClick={handleTaskClick}
                    draggable
                    onDragStart={handleTaskDragStart}
                    style={{
                        color: taskTextColor,
                        backgroundColor: backgroundColor,
                        paddingLeft: 10,
                        paddingTop: 10,
                        paddingBottom: 10,
                    }}>
                    <div style={{ float: "left" }}>
                        <Checkbox
                            checked={isCompleted}
                            onChange={handleCompletedChange}
                            style={{ marginRight: 5 }}
                        />
                        {task.title}
                    </div>
                    <div style={{ float: "right" }}>{taskTagComponents}</div>
                    <br />
                    {overdueComponent}
                    <Activity
                        mode={task.dueDateTime !== null && props.shouldShowFolder
                            ? "visible"
                            : "hidden"}
                    >
                        {" / "}
                    </Activity>
                    <Activity
                        mode={props.shouldShowFolder ? "visible" : "hidden"}
                    >
                        {
                            props.folder
                                ?
                                    <Link to={`/folders/${props.folder.id}`}>
                                        {props.folder.title}
                                    </Link>
                                :
                                    <Link to={"/inbox"}>
                                        {"<Inbox>"}
                                    </Link>
                        }
                    </Activity>
                </div>
            </Dropdown>
            <Divider style={{ marginTop: 0, marginBottom: 0 }} />
        </>
    );
});

export default TaskListItem;
