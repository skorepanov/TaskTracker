import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Checkbox, Divider, Dropdown } from "antd";
import { useStore } from "../../stores/RootStore";
import { formatDate } from "../../utils";
import type { CheckboxChangeEvent } from "antd/es/checkbox";
import { useTranslation } from "../../hooks/useTranslation";
import ITask from "../../interfaces/ITask";
import IFolder from "../../interfaces/IFolder";
import TaskTag from "../tag/TaskTag";

interface ITaskListItemProps {
    task: ITask;
    shouldShowFolder?: boolean;
    folder?: IFolder;
}

const TaskListItem: React.FC<ITaskListItemProps> = observer(props => {
    const { taskStore, tagStore } = useStore();
    const t = useTranslation();

    const { task } = props;

    const [isCompleted, setIsCompleted] = useState<boolean>(
        task.completedDateTime !== null
    );

    const taskTextColor = isCompleted ? "green" : "";

    const backgroundColor =
        taskStore.currentTask?.id === task.id ? "lightgray" : "";

    const folderComponent = props.shouldShowFolder ? (
        <>{props.folder?.title ?? "<Inbox>"}</>
    ) : null;

    const overdueComponentColor =
        !isCompleted && task.overdueDaysCount > 0 ? "red" : "";

    const overdueDaysComponent =
        task.overdueDaysCount > 0 ? (
            <>
                ({task.overdueDaysCount} {t("daysAgo")})
            </>
        ) : null;

    const overdueComponent =
        task.dueDateTime !== null ? (
            <span style={{ color: overdueComponentColor }}>
                {formatDate(task.dueDateTime)} {overdueDaysComponent}
            </span>
        ) : null;

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

    const handleCompletedChange = async (event: CheckboxChangeEvent) => {
        const isCompletedNew = event.target.checked;

        setIsCompleted(isCompletedNew);

        if (isCompletedNew) {
            await taskStore.completeTask(task);
        } else {
            await taskStore.incompleteTask(task);
        }
    };

    const handleMoveTaskToTrashButtonClick = async () => {
        await taskStore.moveTaskToTrash(task);
    };

    const contextMenu = {
        items: [
            {
                key: "moveTaskToTrash",
                label: t("moveTaskToTrash"),
                onClick: handleMoveTaskToTrashButtonClick,
            },
        ],
    };

    return (
        <Dropdown
            key={task.id}
            menu={contextMenu}
            trigger={["contextMenu"]}>
            <div
                onClick={handleTaskClick}
                style={{
                    color: taskTextColor,
                    backgroundColor: backgroundColor,
                    paddingLeft: 10,
                    paddingTop: 5,
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
                {overdueComponent !== null && folderComponent !== null
                    ? " / "
                    : null}
                {folderComponent}
                <Divider size="small" />
            </div>
        </Dropdown>
    );
});

export default TaskListItem;
