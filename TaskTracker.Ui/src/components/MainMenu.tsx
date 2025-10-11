import React from "react";
import { Link, useLocation } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Dropdown, Divider, Menu, MenuProps, Tooltip } from "antd";
import { useStore } from "../stores/RootStore";
import { useTranslation } from "../hooks/useTranslation";
import IFolder from "../interfaces/IFolder";
import ITag from "../interfaces/ITag";
import FolderCreationModalButton from "./folder/FolderCreationModalButton";
import FolderUpdateModalMenuItem from "./folder/FolderUpdateModalMenuItem";
import FolderDeleteModalMenuItem from "./folder/FolderDeleteModalMenuItem";
import TagCreationModalButton from "./tag/TagCreationModalButton";
import TagUpdateModalMenuItem from "./tag/TagUpdateModalMenuItem";
import TagDeleteModalMenuItem from "./tag/TagDeleteModalMenuItem";
import TaskTag from "./tag/TaskTag";
import UserSettingsPanel from "./UserSettingsPanel";

type MenuItem = Required<MenuProps>["items"][number];

const MainMenu: React.FC = observer(() => {
    const location = useLocation();
    const t = useTranslation();

    const { taskStore, folderStore, tagStore } = useStore();

    const allIncompletedTaskCount = taskStore.getIncompletedTasks().length;

    const todayIncompletedTaskCount =
        taskStore.getTodayIncompletedTasks().length;

    const inboxIncompletedTasksCount =
        taskStore.getInboxIncompletedTasks().length;

    const getLabel = (
        link: string,
        title: string,
        incompletedTaskCount?: number
    ) => {
        return (
            <Link to={link}>
                <div style={{ display: "flex", flexDirection: "row" }}>
                    <div
                        style={{
                            flexGrow: 1,
                            overflow: "hidden",
                            textOverflow: "ellipsis",
                        }}
                    >
                        {title}
                    </div>
                    <div style={{ color: "grey", marginLeft: 10 }}>
                        {incompletedTaskCount}
                    </div>
                </div>
            </Link>
        );
    };

    const getFolderRootLabel = () => {
        return (
            <div style={{ display: "flex", flexDirection: "row" }}>
                <div
                    style={{
                        flexGrow: 1,
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                     }}
                >
                    {t("folders")}
                </div>
                <div
                    onClick={e => e.stopPropagation()}
                    style={{ marginLeft: 10, marginRight: 5 }}>
                    <FolderCreationModalButton />
                </div>
            </div>
        );
    };

    const getFolderLabel = (folder: IFolder) => {
        const folderIncompleteTaskCount = taskStore.getFolderIncompletedTasks(
            folder.id
        ).length;

        const contextMenuItems: MenuItem[] = [
            {
                key: "updateFolder",
                label: <FolderUpdateModalMenuItem folder={folder} />,
            },
            {
                key: "deleteFolder",
                label: <FolderDeleteModalMenuItem folder={folder} />,
            },
        ];

        const folderLink = `/folders/${folder.id}`;

        return (
            <Dropdown
                key={folder.id}
                menu={{ items: contextMenuItems }}
                trigger={["contextMenu"]}
            >
                <Tooltip
                    title={folder.title}
                    placement="right"
                    mouseEnterDelay={0.8}
                >
                    <Link to={folderLink}>
                        <div style={{ display: "flex", flexDirection: "row" }}>
                            <div
                                style={{
                                    flexGrow: 1,
                                    overflow: "hidden",
                                    textOverflow: "ellipsis",
                                }}
                            >
                                {folder.title}
                            </div>
                            <div style={{ color: "grey", marginLeft: 10 }}>
                                {folderIncompleteTaskCount}
                            </div>
                        </div>
                    </Link>
                </Tooltip>
            </Dropdown>
        );
    };

    const getFolderChildren = () => {
        return folderStore.getSortedFolders().map(f => {
            return {
                key: `/folders/${f.id}`,
                label: getFolderLabel(f),
            };
        });
    };

    const getTagRootLabel = () => {
        return (
            <div style={{ display: "flex", flexDirection: "row" }}>
                <div
                    style={{
                        flexGrow: 1,
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                    }}
                >
                    {t("tags")}
                </div>
                <div
                    onClick={e => e.stopPropagation()}
                    style={{ marginLeft: 10, marginRight: 5 }}>
                    <TagCreationModalButton />
                </div>
            </div>
        );
    };

    const getTagLabel = (tag: ITag) => {
        const tagIncompleteTaskCount = taskStore.getTagIncompletedTasks(
            tag.id
        ).length;

        const contextMenuItems: MenuItem[] = [
            {
                key: "updateTag",
                label: <TagUpdateModalMenuItem tag={tag} />,
            },
            {
                key: "deleteTag",
                label: <TagDeleteModalMenuItem tag={tag} />,
            },
        ];

        const tagLink = `/tags/${tag.id}`;

        return (
            <Dropdown
                key={tag.id}
                menu={{ items: contextMenuItems }}
                trigger={["contextMenu"]}
            >
                <Tooltip
                    title={tag.title}
                    placement="right"
                    mouseEnterDelay={0.8}
                >
                    <Link to={tagLink}>
                        <div style={{ display: "flex", flexDirection: "row" }}>
                            <div style={{ flexGrow: 1, overflow: "hidden" }}>
                                <TaskTag tag={tag} />
                            </div>
                            <div style={{ color: "grey", marginLeft: 10 }}>
                                {tagIncompleteTaskCount}
                            </div>
                        </div>
                    </Link>
                </Tooltip>
            </Dropdown>
        );
    };

    const getTagChildren = () => {
        return tagStore.getSortedTags().map(t => {
            return {
                key: `/tags/${t.id}`,
                label: getTagLabel(t),
            };
        });
    };

    const mainMenuItems: MenuItem[] = [
        {
            key: "/all",
            label: getLabel("/all", t("allTasks"), allIncompletedTaskCount),
        },
        {
            key: "/today",
            label: getLabel(
                "/today",
                t("todayTasks"),
                todayIncompletedTaskCount
            ),
        },
        {
            key: "/inbox",
            label: getLabel("/inbox", "Inbox", inboxIncompletedTasksCount),
        },
        {
            type: "divider",
        },
        {
            key: "/folders",
            label: getFolderRootLabel(),
            children: getFolderChildren(),
        },
        {
            type: "divider",
        },
        {
            key: "/tags",
            label: getTagRootLabel(),
            children: getTagChildren(),
        },
        {
            type: "divider",
        },
        {
            key: "/trash",
            label: getLabel("/trash", t("tasksInTrash")),
        },
    ];

    const mainMenuDefaultOpenKeys = ["/folders"];

    if (location.pathname.includes("/tags")) {
        mainMenuDefaultOpenKeys.push("/tags");
    }

    return (
        <div
            style={{
                display: "flex",
                flexDirection: "column",
                height: "100%",
            }}>
            <Menu
                items={mainMenuItems}
                mode="inline"
                selectedKeys={[location.pathname]}
                defaultOpenKeys={mainMenuDefaultOpenKeys}
                className="scrollable-container"
                style={{ flexGrow: 1 }}
            />
            <Divider size="small" />
            <UserSettingsPanel />
        </div>
    );
});

export default MainMenu;
